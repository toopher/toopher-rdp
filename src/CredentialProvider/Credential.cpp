/*
	Copyright (c) 2013, pGina Team
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the Toopher Team nor the names of its contributors 
		  may be used to endorse or promote products derived from this software without 
		  specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#include <Windows.h>

#include "Credential.h"
#include "Dll.h"
#include <sddl.h>

#pragma warning(push)
#pragma warning(disable : 4995)
#include <shlwapi.h>
#pragma warning(pop)

#include <Macros.h>

#include "ClassFactory.h"
#include "TileUiTypes.h"
#include "TileUiLogon.h"
#include "TileUiUnlock.h"
#include "TileUiChangePassword.h"
#include "SerializationHelpers.h"
#include "ProviderGuid.h"
#include "resource.h"

#include <wincred.h>

namespace Toopher
{
	namespace CredProv
	{
		IFACEMETHODIMP Credential::QueryInterface(__in REFIID riid, __deref_out void **ppv)
		{
			static const QITAB qitBaseOnly[] =
			{
				QITABENT(Credential, ICredentialProviderCredential),				
				{0},
			};

			static const QITAB qitFull[] =
			{
				QITABENT(Credential, ICredentialProviderCredential),
				QITABENT(Credential, IConnectableCredentialProviderCredential), 
				{0},
			};

			if(m_usageScenario == CPUS_CREDUI)
			{			
				return QISearch(this, qitBaseOnly, riid, ppv);
			}
			else
			{
				return QISearch(this, qitFull, riid, ppv);
			}			
		}

		IFACEMETHODIMP_(ULONG) Credential::AddRef()
		{
	        return InterlockedIncrement(&m_referenceCount);
		}

		IFACEMETHODIMP_(ULONG) Credential::Release()
		{
			LONG count = InterlockedDecrement(&m_referenceCount);
			if (!count)
				delete this;
			return count;
		}

		IFACEMETHODIMP Credential::Advise(__in ICredentialProviderCredentialEvents* pcpce)
		{
			// Release any ref for current ptr (if any)
			UnAdvise();

			m_logonUiCallback = pcpce;
			
			if(m_logonUiCallback)
			{
				m_logonUiCallback->AddRef();			
			}

			return S_OK;
		}
		
		IFACEMETHODIMP Credential::UnAdvise()
		{
			if(m_logonUiCallback)
			{
				m_logonUiCallback->Release();
				m_logonUiCallback = NULL;
			}

			return S_OK;
		}

		IFACEMETHODIMP Credential::SetSelected(__out BOOL* pbAutoLogon)
		{
			// We don't do anything special here, but twould be the place to react to our tile being selected
			*pbAutoLogon = FALSE;
			return S_OK;
		}

		IFACEMETHODIMP Credential::SetDeselected()
		{
			// No longer selected, if we have any password fields set, lets zero/clear/free them
			ClearZeroAndFreeAnyPasswordFields(true);
			return S_OK;
		}

		IFACEMETHODIMP Credential::GetFieldState(__in DWORD dwFieldID, __out CREDENTIAL_PROVIDER_FIELD_STATE* pcpfs, __out CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE* pcpfis)
		{			
			if(!m_fields || dwFieldID >= m_fields->fieldCount || !pcpfs || !pcpfis)
				return E_INVALIDARG;

			*pcpfs = m_fields->fields[dwFieldID].fieldStatePair.fieldState;
			*pcpfis = m_fields->fields[dwFieldID].fieldStatePair.fieldInteractiveState;
			return S_OK;
		}
		
		IFACEMETHODIMP Credential::GetStringValue(__in DWORD dwFieldID, __deref_out PWSTR* ppwsz)
		{
			if(!m_fields || dwFieldID >= m_fields->fieldCount || !ppwsz)
				return E_INVALIDARG;

			// We copy our value with SHStrDupW which uses CoTask alloc, caller owns result
			if(m_fields->fields[dwFieldID].wstr)
				return SHStrDupW(m_fields->fields[dwFieldID].wstr, ppwsz);

			*ppwsz = NULL;
			return S_OK;			
		}

		IFACEMETHODIMP Credential::GetBitmapValue(__in DWORD dwFieldID, __out HBITMAP* phbmp)
		{
			if(!m_fields || dwFieldID >= m_fields->fieldCount || !phbmp)
				return E_INVALIDARG;

			if(m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_TILE_IMAGE)
				return E_INVALIDARG;

			HBITMAP bitmap = NULL;
			std::wstring tileImage = Toopher::Registry::GetString(L"TileImage", L"");
			if(tileImage.empty() || tileImage.length() == 1)
			{
				// Use builtin
				bitmap = LoadBitmap(GetMyInstance(), MAKEINTRESOURCE(IDB_LOGO_MONOCHROME_200));
			}
			else
			{
				pDEBUG(L"Credential::GetBitmapValue: Loading image from: %s", tileImage.c_str());
				bitmap = (HBITMAP) LoadImageW((HINSTANCE) NULL, tileImage.c_str(), IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);			
			}
			
			if(!bitmap)
				return HRESULT_FROM_WIN32(GetLastError());
			
			*phbmp = bitmap;
			return S_OK;
		}

		IFACEMETHODIMP Credential::GetCheckboxValue(__in DWORD dwFieldID, __out BOOL* pbChecked, __deref_out PWSTR* ppwszLabel)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::GetComboBoxValueCount(__in DWORD dwFieldID, __out DWORD* pcItems, __out_range(<,*pcItems) DWORD* pdwSelectedItem)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::GetComboBoxValueAt(__in DWORD dwFieldID, __in DWORD dwItem, __deref_out PWSTR* ppwszItem)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::GetSubmitButtonValue(__in DWORD dwFieldID, __out DWORD* pdwAdjacentTo)
		{
			if(!m_fields || dwFieldID >= m_fields->fieldCount || !pdwAdjacentTo)
				return E_INVALIDARG;

			if(m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_SUBMIT_BUTTON)
				return E_INVALIDARG;

			*pdwAdjacentTo = m_fields->submitAdjacentTo;
			return S_OK;
		}

		IFACEMETHODIMP Credential::SetStringValue(__in DWORD dwFieldID, __in PCWSTR pwz)
		{			
			if(!m_fields || dwFieldID >= m_fields->fieldCount)
				return E_INVALIDARG;

			if(m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_EDIT_TEXT &&
			   m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_PASSWORD_TEXT &&
			   m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_SMALL_TEXT &&
			   m_fields->fields[dwFieldID].fieldDescriptor.cpft != CPFT_LARGE_TEXT)
				return E_INVALIDARG;

			if(m_fields->fields[dwFieldID].wstr)
			{
				CoTaskMemFree(m_fields->fields[dwFieldID].wstr);
				m_fields->fields[dwFieldID].wstr = NULL;
			}
			
			if(pwz)
			{
				return SHStrDupW(pwz, &m_fields->fields[dwFieldID].wstr);
			}
			return S_OK;
		}

		IFACEMETHODIMP Credential::SetCheckboxValue(__in DWORD dwFieldID, __in BOOL bChecked)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::SetComboBoxSelectedValue(__in DWORD dwFieldID, __in DWORD dwSelectedItem)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::CommandLinkClicked(__in DWORD dwFieldID)
		{
			return E_NOTIMPL;
		}

		IFACEMETHODIMP Credential::GetSerialization(__out CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE* pcpgsr, __out CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION* pcpcs,
													__deref_out_opt PWSTR* ppwszOptionalStatusText, __out CREDENTIAL_PROVIDER_STATUS_ICON* pcpsiOptionalStatusIcon)
		{
			pDEBUG(L"Credential::GetSerialization, enter");

			// If we are operating in a CPUS_LOGON, CPUS_CHANGE_PASSWORD or CPUS_UNLOCK_WORKSTATION scenario, then 
			// Credential::Connect will have executed prior to this method, which calls
			// ProcessLoginAttempt, so m_loginResult should have the result from the plugins. 
			// Otherwise, we need to execute plugins for the appropriate scenario.
			if(m_usageScenario == CPUS_CREDUI)
			{
				ProcessLoginAttempt(NULL);
			}

			if( m_logonCancelled )
			{
				// User clicked cancel during logon
				pDEBUG(L"Credential::GetSerialization - Logon was cancelled, returning S_FALSE");
				SHStrDupW(L"Logon cancelled", ppwszOptionalStatusText);
				*pcpgsr = CPGSR_NO_CREDENTIAL_FINISHED;										
				*pcpsiOptionalStatusIcon = CPSI_ERROR;
				return S_FALSE;
			}

			if(!m_loginResult.Result())
			{
				pERROR(L"Credential::GetSerialization: Failed attempt");
				if(m_loginResult.Message().length() > 0)
				{
					SHStrDupW(m_loginResult.Message().c_str(), ppwszOptionalStatusText);					
				}
				else
				{
					SHStrDupW(L"Plugins did not provide a specific error message", ppwszOptionalStatusText);
				}
				
				*pcpgsr = CPGSR_NO_CREDENTIAL_FINISHED;
				*pcpsiOptionalStatusIcon = CPSI_ERROR;
				return S_FALSE;
			}

			// If this is the change password scenario, we don't want to continue any 
			// further.  Just notify the user that the change was successful, and return
			// false, because we don't want Windows to actually process this change.  It was already
			// processed by the plugins, so there's nothing more to do.
			if( m_loginResult.Result() && CPUS_CHANGE_PASSWORD == m_usageScenario ) {
				if(m_loginResult.Message().length() > 0)
				{
					SHStrDupW(m_loginResult.Message().c_str(), ppwszOptionalStatusText);					
				}
				else
				{
					SHStrDupW(L"Toopher: Your password was successfully changed", ppwszOptionalStatusText);
				}

				*pcpgsr = CPGSR_NO_CREDENTIAL_FINISHED;						
				*pcpsiOptionalStatusIcon = CPSI_SUCCESS;
				return S_FALSE;
			}

			// At this point we have a successful logon, and we're not in the 
			// change password scenario.  The successful login info is validated and available
			// in m_loginResult. So now we pack it up and provide it back to
			// LogonUI/Winlogon as a serialized/packed structure.
			pDEBUG(L"We have a successful logon");

			Toopher::Memory::ObjectCleanupPool cleanup;

			PWSTR username = m_loginResult.Username().length() > 0 ? _wcsdup(m_loginResult.Username().c_str()) : NULL;
			PWSTR password = m_loginResult.Password().length() > 0 ? _wcsdup(m_loginResult.Password().c_str()) : NULL;
			PWSTR domain = m_loginResult.Domain().length() > 0 ? _wcsdup(m_loginResult.Domain().c_str()) : NULL;			

			cleanup.AddFree(username);
			cleanup.AddFree(password);
			cleanup.AddFree(domain);

			PWSTR protectedPassword = NULL;			
			HRESULT result = Microsoft::Sample::ProtectIfNecessaryAndCopyPassword(password, m_usageScenario, &protectedPassword);			
			if(!SUCCEEDED(result))
				return result;

			cleanup.Add(new Toopher::Memory::CoTaskMemFreeCleanup(protectedPassword));			

			// CredUI we use CredPackAuthenticationBuffer
			if(m_usageScenario == CPUS_CREDUI)
			{
				pDEBUG(L"CREDUI scenario");
				// Need username/domain as a single string
				PWSTR domainUsername = NULL;
				result = Microsoft::Sample::DomainUsernameStringAlloc(domain, username, &domainUsername);
				if(SUCCEEDED(result))
				{
					DWORD size = 0;
					BYTE* rawbits = NULL;
					
					if(!CredPackAuthenticationBufferW((CREDUIWIN_PACK_32_WOW & m_usageFlags) ? CRED_PACK_WOW_BUFFER : 0, domainUsername, protectedPassword, rawbits, &size))
					{
						if(GetLastError() == ERROR_INSUFFICIENT_BUFFER)
						{
							rawbits = (BYTE *)HeapAlloc(GetProcessHeap(), 0, size);
							if(!CredPackAuthenticationBufferW((CREDUIWIN_PACK_32_WOW & m_usageFlags) ? CRED_PACK_WOW_BUFFER : 0, domainUsername, protectedPassword, rawbits, &size))
							{
								HeapFree(GetProcessHeap(), 0, rawbits);
								HeapFree(GetProcessHeap(), 0, domainUsername);
								return HRESULT_FROM_WIN32(GetLastError());
							}

							pcpcs->rgbSerialization = rawbits;
							pcpcs->cbSerialization = size;
						}
						else
						{
							HeapFree(GetProcessHeap(), 0, domainUsername);
							return E_FAIL;
						}
					}
				}
			}
			else if( CPUS_LOGON == m_usageScenario || CPUS_UNLOCK_WORKSTATION == m_usageScenario )
			{
				pDEBUG(L"LOGON or UNLOCK_WORKSTATION scenario");
				// Init kiul
				KERB_INTERACTIVE_UNLOCK_LOGON kiul;
				result = Microsoft::Sample::KerbInteractiveUnlockLogonInit(domain, username, password, m_usageScenario, &kiul);
				if(!SUCCEEDED(result)) {
					pDEBUG(L"Not Succeeded(result) - 1");
					return result;
				}

				// Pack for the negotiate package and include our CLSID
				result = Microsoft::Sample::KerbInteractiveUnlockLogonPack(kiul, &pcpcs->rgbSerialization, &pcpcs->cbSerialization);
				if(!SUCCEEDED(result)) {
					pDEBUG(L"Not Succeeded(result) - 2");
					return result;
				}
			}
			
			ULONG authPackage = 0;
			result = Microsoft::Sample::RetrieveNegotiateAuthPackage(&authPackage);
			if(!SUCCEEDED(result)) {
				pDEBUG(L"Not succeeded(result) - 3");
				return result;
			}
						
			pcpcs->ulAuthenticationPackage = authPackage;
			pcpcs->clsidCredentialProvider = CLSID_CToopherProvider;
			*pcpgsr = CPGSR_RETURN_CREDENTIAL_FINISHED;            
            
			return S_OK;
        }
    
		IFACEMETHODIMP Credential::ReportResult(__in NTSTATUS ntsStatus, __in NTSTATUS ntsSubstatus, 
												__deref_out_opt PWSTR* ppwszOptionalStatusText, 
												__out CREDENTIAL_PROVIDER_STATUS_ICON* pcpsiOptionalStatusIcon)
		{
			pDEBUG(L"Credential::ReportResult(0x%08x, 0x%08x) called", ntsStatus, ntsSubstatus);
			/**ppwszOptionalStatusText = NULL;
			*pcpsiOptionalStatusIcon = CPSI_NONE;*/
			return E_NOTIMPL;
		}

		Credential::Credential() :
			m_referenceCount(1),
			m_usageScenario(CPUS_INVALID),
			m_logonUiCallback(NULL),
			m_fields(NULL),			
			m_usageFlags(0),
			m_logonCancelled(false)
		{
			pDEBUG(L"Credential::Credential()");
			AddDllReference();

		}
		
		Credential::~Credential()
		{
			pDEBUG(L"Credential::~Credential()");
			ClearZeroAndFreeAnyTextFields(false);	// Free memory used to back text fields, no ui update
			ReleaseDllReference();
			pDEBUG(L"Credential::~Credential() : Done");
		}

		void Credential::Initialize(CREDENTIAL_PROVIDER_USAGE_SCENARIO cpus, UI_FIELDS const& fields, DWORD usageFlags, const wchar_t *username, const wchar_t *password)
		{
			pDEBUG(L"Credential::Initialize - cpus = %d, usageFlags = %d, username = %s, password = *****", cpus, usageFlags, username);
			m_usageScenario = cpus;
			m_usageFlags = usageFlags;

			// Allocate and copy our UI_FIELDS struct, we need our own copy to set/track the state of
			//  our fields over time
			m_fields = (UI_FIELDS *) (malloc(sizeof(UI_FIELDS) + (sizeof(UI_FIELD) * fields.fieldCount)));
			m_fields->fieldCount = fields.fieldCount;
			m_fields->submitAdjacentTo = fields.submitAdjacentTo;
			m_fields->usernameFieldIdx = fields.usernameFieldIdx;
			m_fields->passwordFieldIdx = fields.passwordFieldIdx;
			m_fields->statusFieldIdx = fields.statusFieldIdx;
			for(DWORD x = 0; x < fields.fieldCount; x++)
			{
				m_fields->fields[x].fieldDescriptor = fields.fields[x].fieldDescriptor;
				m_fields->fields[x].fieldStatePair = fields.fields[x].fieldStatePair;
				m_fields->fields[x].fieldDataSource = fields.fields[x].fieldDataSource;
				m_fields->fields[x].wstr = NULL;

				if(fields.fields[x].wstr)
				{
					SHStrDup(fields.fields[x].wstr, &m_fields->fields[x].wstr);
				}

			}			

			// Fill the username field (if necessary)
			if(username != NULL)
			{				
				SHStrDupW(username, &(m_fields->fields[m_fields->usernameFieldIdx].wstr));

				// If the username field has focus, hand focus over to the password field
				if(m_fields->fields[m_fields->usernameFieldIdx].fieldStatePair.fieldInteractiveState == CPFIS_FOCUSED) {
					m_fields->fields[m_fields->usernameFieldIdx].fieldStatePair.fieldInteractiveState = CPFIS_NONE;
					m_fields->fields[m_fields->passwordFieldIdx].fieldStatePair.fieldInteractiveState = CPFIS_FOCUSED;
				}
			}
			else if(m_usageScenario == CPUS_UNLOCK_WORKSTATION)
			{
				DWORD mySession = Toopher::Helpers::GetCurrentSessionId();
				std::wstring sessionUname, domain;    // Username and domain to be determined
				std::wstring usernameFieldValue;  // The value for the username field
				std::wstring machineName = Toopher::Helpers::GetMachineName();

				// Get user information from service (if available)
				pDEBUG(L"Retrieving user information from service.");
				Toopher::Transactions::LoginInfo::UserInformation userInfo = Toopher::Transactions::LoginInfo::UserInformation();
				
				pDEBUG(L"Received: original uname: '%s' uname: '%s' domain: '%s'", 
					userInfo.OriginalUsername().c_str(), userInfo.Username().c_str(), userInfo.Domain().c_str());

				// Grab the domain if available
				if( ! userInfo.Domain().empty() )
					domain = userInfo.Domain();

				// Are we configured to use the original username?
				if( Toopher::Registry::GetBool(L"UseOriginalUsernameInUnlockScenario", false) )
					sessionUname = userInfo.OriginalUsername();
				else
					sessionUname = userInfo.Username();

				// If we didn't get a username/domain from the service, try to get it from WTS
				if( sessionUname.empty() )
					sessionUname = Toopher::Helpers::GetSessionUsername(mySession);
				if( domain.empty() )
					domain = Toopher::Helpers::GetSessionDomainName(mySession);
					

				if(!domain.empty() && _wcsicmp(domain.c_str(), machineName.c_str()) != 0)
				{
					usernameFieldValue += domain;
					usernameFieldValue += L"\\";
				}

				usernameFieldValue += sessionUname;
				
				SHStrDupW(usernameFieldValue.c_str(), &(m_fields->fields[m_fields->usernameFieldIdx].wstr));
			} else if( CPUS_CHANGE_PASSWORD == m_usageScenario ) {
				DWORD mySession = Toopher::Helpers::GetCurrentSessionId();

				std::wstring sessionUname = Toopher::Helpers::GetSessionUsername(mySession);

				SHStrDupW(sessionUname.c_str(), &(m_fields->fields[m_fields->usernameFieldIdx].wstr));
			}

			if(password != NULL)
			{	
				SHStrDupW(password, &(m_fields->fields[m_fields->passwordFieldIdx].wstr));
			}

			// Hide MOTD field if not enabled
			if( ! Toopher::Registry::GetBool(L"EnableMotd", true) )
				if( m_usageScenario == CPUS_LOGON )
					m_fields->fields[CredProv::LUIFI_MOTD].fieldStatePair.fieldState = CPFS_HIDDEN;

			// Hide service status if configured to do so
			if( ! Toopher::Registry::GetBool(L"ShowServiceStatusInLogonUi", true) )
			{
				if( m_usageScenario == CPUS_UNLOCK_WORKSTATION )
					m_fields->fields[CredProv::LOIFI_STATUS].fieldStatePair.fieldState = CPFS_HIDDEN;
				else if( m_usageScenario == CPUS_LOGON )
					m_fields->fields[CredProv::LUIFI_STATUS].fieldStatePair.fieldState = CPFS_HIDDEN;
			}
		}

		void Credential::ClearZeroAndFreeAnyPasswordFields(bool updateUi)
		{
			ClearZeroAndFreeFields(CPFT_PASSWORD_TEXT, updateUi);					
    	}

		void Credential::ClearZeroAndFreeAnyTextFields(bool updateUi)
		{
			ClearZeroAndFreeFields(CPFT_PASSWORD_TEXT, updateUi);
			ClearZeroAndFreeFields(CPFT_EDIT_TEXT, updateUi);
		}

		void Credential::ClearZeroAndFreeFields(CREDENTIAL_PROVIDER_FIELD_TYPE type, bool updateUi)
		{
			if(!m_fields) return;

			for(DWORD x = 0; x < m_fields->fieldCount; x++)
			{
				if(m_fields->fields[x].fieldDescriptor.cpft == type)
				{
					if(m_fields->fields[x].wstr)
					{
						size_t len = wcslen(m_fields->fields[x].wstr);
						SecureZeroMemory(m_fields->fields[x].wstr, len * sizeof(wchar_t));
						CoTaskMemFree(m_fields->fields[x].wstr);						
						m_fields->fields[x].wstr = NULL;

						// If we've been advised, we can tell the UI so the UI correctly reflects that this
						//	field is not set any longer (set it to empty string)
						if(m_logonUiCallback && updateUi)
						{
							m_logonUiCallback->SetFieldString(this, m_fields->fields[x].fieldDescriptor.dwFieldID, L"");
						}
					}
				}
			}	
		}

		PWSTR Credential::FindUsernameValue()
		{
			if(!m_fields) return NULL;
			return m_fields->fields[m_fields->usernameFieldIdx].wstr;
		}

		PWSTR Credential::FindPasswordValue()
		{
			if(!m_fields) return NULL;			
			return m_fields->fields[m_fields->passwordFieldIdx].wstr;
		}

		DWORD Credential::FindStatusId()
		{
			if(!m_fields) return 0;
			return m_fields->statusFieldIdx;
		}

		std::wstring Credential::GetTextForField(DWORD dwFieldID)
		{
			// Retrieve data for dynamic fields
			if(m_fields->fields[dwFieldID].fieldDataSource == SOURCE_CALLBACK && m_fields->fields[dwFieldID].labelCallback != NULL)
			{
				return m_fields->fields[dwFieldID].labelCallback(m_fields->fields[dwFieldID].fieldDescriptor.pszLabel, m_fields->fields[dwFieldID].fieldDescriptor.dwFieldID);				
			}
			

			return L"";
		}

		// Called just after the "submit" button is clicked and just before GetSerialization
		IFACEMETHODIMP Credential::Connect( IQueryContinueWithStatus *pqcws )
		{
			pDEBUG(L"Credential::Connect()");
			if( CPUS_CREDUI == m_usageScenario || CPUS_LOGON == m_usageScenario ) {
				ProcessLoginAttempt(pqcws);
			} else if( CPUS_CHANGE_PASSWORD == m_usageScenario ) {
				ProcessChangePasswordAttempt();
			}
			
			return S_OK;
		}

		IFACEMETHODIMP Credential::Disconnect()
		{
			return E_NOTIMPL;
		}

		LPTSTR GetLastErrorString(int hresult)
		{
			LPTSTR errorText = NULL;

			FormatMessage(
				// use system message tables to retrieve error text
				FORMAT_MESSAGE_FROM_SYSTEM
				// allocate buffer on local heap for error text
				|FORMAT_MESSAGE_ALLOCATE_BUFFER
				// Important! will fail otherwise, since we're not 
				// (and CANNOT) pass insertion parameters
				|FORMAT_MESSAGE_IGNORE_INSERTS,  
				NULL,    // unused with FORMAT_MESSAGE_FROM_SYSTEM
				   
				hresult,
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
				(LPTSTR)&errorText,  // output 
				0, // minimum size for output buffer
				NULL);   // arguments - see note 

			return errorText;
		}

		void PrintLastError()
		{
			int hresult = GetLastError();

			LPTSTR errorText = GetLastErrorString(hresult);

			if ( NULL != errorText )
			{
				// ... do something with the string - log it, display it to the user, etc.
				pDEBUG(L"    PrintLastError: %d - %s", hresult, errorText);
			
				// release memory allocated by FormatMessage()
				LocalFree(errorText);
				errorText = NULL;
			} else {
				pDEBUG(L"    PrintLastError: %d - [failed to get error string]", hresult);
			}
		}

				/* static */
		bool LoginForUser(const wchar_t *lpwLoginDomain, const wchar_t *lpwUserName, const wchar_t *lpwPassword, wchar_t *lpwResolvedDomain, DWORD *lpdLen, wchar_t *lpwUpn, DWORD *lpUpnLen)
		{
			if (lpwLoginDomain) {
				pDEBUG(L"Using LogonUser(username = %s,domain = %s, [password provided])", lpwUserName, lpwLoginDomain);
			} else {
				pDEBUG(L"Using LogonUser(username = %s,domain = NULL, [password provided])", lpwUserName);
			}
			HANDLE token = NULL;
			BOOL logonResult = LogonUser(lpwUserName, lpwLoginDomain, lpwPassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, &token);
			if(logonResult == TRUE)				
			{
				if (lpwUpn != NULL) {
					// impersonate user so we can look up their UPN
					if (ImpersonateLoggedOnUser(token)) {
						__try {
				
							// get full user principal name
							if(GetUserNameEx(NameUserPrincipal, lpwUpn, lpUpnLen)) {
								if (lpwResolvedDomain != NULL) {
									// if that succeeds and the user gave us a pointer for the resolved domain, get the domain name for the returned UPN
									// we're not actually going to use the SID structure, but it has to be defined for the LookupAccountName call
									PSID pSid = (PSID) new BYTE[100];
									if (pSid) {
										__try {
											memset(pSid, 0, 100);
											DWORD cbSid = 100;
											SID_NAME_USE eSidType;
											BOOL result = LookupAccountNameW(NULL, lpwUpn, pSid, &cbSid, lpwResolvedDomain, lpdLen, &eSidType);
										}
										__finally {
											delete [] pSid;
										}
									}
								}
							} else {
								PrintLastError();
							}
						}
						__finally {
							RevertToSelf();
						}
					} else {
						PrintLastError();
					}
				}

				CloseHandle(token);
				return true;
			}
			return false;
		}

		
		void Credential::ProcessLoginAttempt(IQueryContinueWithStatus *pqcws)
		{
			pDEBUG(L"ProcessLoginAttempt");
			// Reset m_loginResult
			m_loginResult.Clear();
			m_logonCancelled = false;

			// Workout what our username, and password are.  Plugins are responsible for
			// parsing out domain\machine name if needed
			const wchar_t *pDomainUsername = FindUsernameValue();
			std::wstring domainUsername = std::wstring(pDomainUsername);
			std::wstring sUsername;
			std::wstring sDomain;
			pDEBUG(L"  domainUsername is %s", domainUsername.c_str());
			
			const wchar_t *username = NULL;
			const wchar_t *domain = NULL;
			
			size_t slashPos = domainUsername.find(L"\\");
			if (slashPos == domainUsername.npos) {
				// raw username, or UPN username - domain should be empty, and LogonUser will figure out the domain
				sUsername = domainUsername;
			} else {
				// down-level logon name - domain\username
				sUsername = domainUsername.substr(slashPos+1);
				sDomain = domainUsername.substr(0, slashPos);
				domain = sDomain.c_str();
			}
			username = sUsername.c_str();
				
			
			PWSTR password = FindPasswordValue();
			
			Toopher::Protocol::LoginRequestMessage::LoginReason reason = Toopher::Protocol::LoginRequestMessage::Login;
			switch(m_usageScenario)
			{
			case CPUS_LOGON:
				break;
			case CPUS_UNLOCK_WORKSTATION:
				reason = Toopher::Protocol::LoginRequestMessage::Unlock;
				break;
			case CPUS_CREDUI:
				reason = Toopher::Protocol::LoginRequestMessage::CredUI;
				break;
			}

			if (domain) {
				pDEBUG(L"  ProcessLoginAttempt: Processing login for %s\\%s", domain, username);
			} else {
				pDEBUG(L"  ProcessLoginAttempt: Processing login for %s", username);
			}
			
			// Set the status message
			if( pqcws )
			{
				std::wstring message = Toopher::Registry::GetString(L"LogonProgressMessage", L"Logging on...");

				// Replace occurences of %u with the username
				std::wstring unameCopy = username;
				std::wstring::size_type unameSize = unameCopy.size();
				for( std::wstring::size_type pos = 0; 
					(pos = message.find(L"%u", pos)) != std::wstring::npos;
					pos += unameSize )
				{
					message.replace(pos, unameSize, unameCopy);
				}

				pqcws->SetStatusMessage(message.c_str());
			}

			//bool LoginForUser(const wchar_t *lpwLoginDomain, const wchar_t *lpwUserName, const wchar_t *lpwPassword, wchar_t *lpwResolvedDomain, DWORD &lpdLen, wchar_t *lpwUpn, DWORD &lpUpnLen)
		
			const int defaultBufferLen = 255;
			DWORD resolvedDomainLen = defaultBufferLen;
			wchar_t *resolvedDomain = new wchar_t[resolvedDomainLen];
			DWORD userUpnLen = defaultBufferLen;
			wchar_t *userUpn = new wchar_t[userUpnLen];

			if(LoginForUser(domain, username, password, resolvedDomain, &resolvedDomainLen, userUpn, &userUpnLen))
			{
				pDEBUG(L"  Local login succeeded.  upn = %s, resolved domain = %s", userUpn, resolvedDomain);
				if (!Toopher::Helpers::UserIsRemote()) {
					m_loginResult = Toopher::Transactions::User::LoginResult(true, username, password, domain, L"Local login bypasses Toopher Authentication");
				} else {
					// we only want to be active for remote connections

					std::wstring cmd_app_ws = Toopher::Registry::GetString(L"ToopherAuthExePath", L"");
					std::wstring cmd_ws = cmd_app_ws + L" " + std::wstring(username);
					pDEBUG(L"  launching command: %s", cmd_ws.c_str());
				
					STARTUPINFO si;
					PROCESS_INFORMATION pi;
					ZeroMemory( &si, sizeof(si));
					si.cb = sizeof(si);
					ZeroMemory( &pi, sizeof(pi));
					if (!CreateProcess( NULL,
							(LPWSTR)cmd_ws.c_str(),
							NULL,
							NULL,
							FALSE,
							0,
							NULL,
							NULL,
							&si,
							&pi))
					{
						pDEBUG(L"  Failed to launch ToopherAuth process");
						PrintLastError();
						m_loginResult = Toopher::Transactions::User::LoginResult(false, username, password, resolvedDomain, L"Failed to authenticate with Toopher");
					}
					else
					{
						WaitForSingleObject(pi.hProcess, INFINITE);
						DWORD exitCode;
						if (FALSE == GetExitCodeProcess(pi.hProcess, &exitCode)) {
							m_loginResult = Toopher::Transactions::User::LoginResult(false, username, password, resolvedDomain, L"Failed to get Toopher authentication result");
						} else {
							pDEBUG(L"  Return code: %d", exitCode);
			
							m_loginResult = Toopher::Transactions::User::LoginResult(exitCode == 0, username, password, resolvedDomain, L"Authentication Denied by Toopher");
						}
					}
				}
				
			}
			else
			{
				std::wstring errorString = GetLastErrorString(GetLastError());
				m_loginResult = Toopher::Transactions::User::LoginResult(false, username, password, L"", errorString);
				pDEBUG(L"  Local login failed: %s", errorString.c_str());
			}

			if (resolvedDomain) {
				delete [] resolvedDomain;
			}
			if (userUpn) {
				delete [] userUpn;
			}
			
			
			
			if( pqcws )
			{
				if( m_loginResult.Result() )
				{
					pDEBUG(L"  logon success");
					pqcws->SetStatusMessage(L"Logon successful");
				}
				else
				{
					pDEBUG(L"  logon failure");
					pqcws->SetStatusMessage(L"Logon failed");
				}

				// Did the user click the "Cancel" button?
				if( pqcws->QueryContinue() != S_OK )
				{
					pDEBUG(L"  User clicked cancel button during plugin processing");
					m_logonCancelled = true;
				}
			}			
		}

		void Credential::ProcessChangePasswordAttempt() 
		{
			pDEBUG(L"ProcessChangePasswordAttempt()");
			m_loginResult.Clear();
			m_logonCancelled = false;

			// Get strings from fields
			PWSTR username = FindUsernameValue();			
			PWSTR oldPassword = FindPasswordValue();  // This is the old password
			
			// Find the new password and confirm new password fields
			PWSTR newPassword = NULL;
			PWSTR newPasswordConfirm = NULL;
			if(m_fields) {
				newPassword = m_fields->fields[CredProv::CPUIFI_NEW_PASSWORD].wstr;
				newPasswordConfirm = m_fields->fields[CredProv::CPUIFI_CONFIRM_NEW_PASSWORD].wstr;
			}
			
			// Check that the new password and confirmation are exactly the same, if not
			// return a failure.
			if( wcscmp(newPassword, newPasswordConfirm ) != 0 ) {
				m_loginResult.Result(false);
				m_loginResult.Message(L"New passwords do not match");
				return;
			}

			MessageBox(0, L"Password Change Not Supported", L"Not Implemented", MB_OK);
			m_loginResult = Toopher::Transactions::User::LoginResult(false, username, oldPassword, Toopher::Helpers::GetMachineName(), L"");
			
			if( m_loginResult.Message().empty() ) {
				if( m_loginResult.Result() )
					m_loginResult.Message(L"Password was successfully changed");
				else
					m_loginResult.Message(L"Failed to change password, no message from plugins.");
			}
		}
	}
}

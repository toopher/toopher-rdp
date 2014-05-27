/*
	Copyright (c) 2011, pGina Team
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
#include <stdio.h>
#include <fstream>

#include "pGinaTransactions.h"
#include "ObjectCleanupPool.h"
#include "PipeClient.h"
#include "Message.h"
#include "pGinaMessages.h"
#include "Registry.h"
#include "Helpers.h"

namespace Toopher
{
	namespace Transactions
	{			
		/* static */
		void Log::Debug(const wchar_t *format, ...)
		{
			wchar_t buffer[2048];
			memset(buffer, 0, sizeof(buffer));
			
			va_list args;
			va_start(args, format);
			_vsnwprintf_s( buffer, sizeof(buffer) / sizeof(WORD), _TRUNCATE, format, args);

			LogInternal(L"Debug", buffer);
		}

		/* static */
		void Log::Info(const wchar_t *format, ...)
		{
			wchar_t buffer[2048];
			memset(buffer, 0, sizeof(buffer));
			
			va_list args;
			va_start(args, format);
			_vsnwprintf_s( buffer, sizeof(buffer) / sizeof(WORD), _TRUNCATE, format, args);

			LogInternal(L"Info", buffer);
		}

		/* static */
		void Log::Warn(const wchar_t *format, ...)
		{
			wchar_t buffer[2048];
			memset(buffer, 0, sizeof(buffer));
			
			va_list args;
			va_start(args, format);
			_vsnwprintf_s( buffer, sizeof(buffer) / sizeof(WORD), _TRUNCATE, format, args);

			LogInternal(L"Warn", buffer);
		}

		/* static */
		void Log::Error(const wchar_t *format, ...)
		{
			wchar_t buffer[2048];
			memset(buffer, 0, sizeof(buffer));
			
			va_list args;
			va_start(args, format);
			_vsnwprintf_s( buffer, sizeof(buffer) / sizeof(WORD), _TRUNCATE, format, args);

			LogInternal(L"Error", buffer);
		}
		
		/* static */
		void Log::LogInternal(const wchar_t *level, const wchar_t *message)
		{
			// Call outputdebugstring, after appending a \n and truncating
			wchar_t ods_buffer[4096 - 6];	// Max output to OutputDebugString
			_snwprintf_s(ods_buffer, sizeof(ods_buffer) / sizeof(WORD), _TRUNCATE, L"%s\n", message);
			if (Toopher::Registry::GetBool(L"DebugMode", false)) {
				std::wofstream outfile;
				outfile.open("c:\\ToopherCredentialProvider.log", std::ios_base::app);
				outfile << ods_buffer;
				outfile.flush();
			}
			OutputDebugString(ods_buffer);
		}

	}
}
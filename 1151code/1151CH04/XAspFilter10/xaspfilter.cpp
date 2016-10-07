// xaspfilter.cpp - Implementation file for ISAPI
// Copyright (c) 2003, Christoph Wille, christophw@alphasierrapapa.com
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are 
// permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list 
//   of conditions and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list
//   of conditions and the following disclaimer in the documentation and/or other materials 
//   provided with the distribution.
//
// - Neither the name of the AlphaSierraPapa nor the names of its contributors may be used to 
//   endorse or promote products derived from this software without specific prior written 
//   permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS 
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
// IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#include "stdafx.h"
#include "xaspfilter.h"


// The one and only CXAspxFilter object
CXAspxFilter theFilter;



// CXAspxFilter implementation

CXAspxFilter::CXAspxFilter()
{
}

CXAspxFilter::~CXAspxFilter()
{
}

BOOL CXAspxFilter::GetFilterVersion(PHTTP_FILTER_VERSION pVer)
{
	// Call default implementation for initialization
	CHttpFilter::GetFilterVersion(pVer);

	// Clear the flags set by base class
	pVer->dwFlags &= ~SF_NOTIFY_ORDER_MASK;

	// Set the flags we are interested in
	pVer->dwFlags |= SF_NOTIFY_SECURE_PORT | SF_NOTIFY_NONSECURE_PORT | SF_NOTIFY_SEND_RESPONSE;

	// Set Priority
	pVer->dwFlags |= SF_NOTIFY_ORDER_HIGH;

	// Load description string
	TCHAR sz[SF_MAX_FILTER_DESC_LEN+1];
	ISAPIVERIFY(::LoadString(AfxGetResourceHandle(),
			IDS_FILTER, sz, SF_MAX_FILTER_DESC_LEN));
	_tcscpy(pVer->lpszFilterDesc, sz);
	return TRUE;
}

//  This is how a standard HTTP header response looks like with ASP.NET 1.1:
//
//  HTTP/1.1 200 OK
//  Server: Microsoft-IIS/5.0
//  Date: Fri, 16 May 2003 12:07:29 GMT
//  X-Powered-By: ASP.NET
//  X-AspNet-Version: 1.1.4322
//  Set-Cookie: ASP.NET_SessionId=jcgilz55wur2wtqc4fh54g55; path=/
//  Cache-Control: private
//  Content-Type: text/html; charset=utf-8
//
//  OnSendResponse deletes the two X- headers to mask what is running on the machine

DWORD CXAspxFilter::OnSendResponse(CHttpFilterContext* pCtxt, PHTTP_FILTER_SEND_RESPONSE pSendResponse)
{
	PHTTP_FILTER_CONTEXT pfc = pCtxt->m_pFC;
	pSendResponse->SetHeader(pfc, _T("X-Powered-By:"), '\0'); // '\0' means delete header altogether

	
	/*<!--
    httpRuntime Attributes:
        executionTimeout="[seconds]" - time in seconds before request is automatically timed out
        maxRequestLength="[KBytes]" - KBytes size of maximum request length to accept
        useFullyQualifiedRedirectUrl="[true|false]" - Fully qualifiy the URL for client redirects
        minFreeThreads="[count]" - minimum number of free thread to allow execution of new requests
        minLocalRequestFreeThreads="[count]" - minimum number of free thread to allow execution of new local requests
        appRequestQueueLimit="[count]" - maximum number of requests queued for the application
        enableKernelOutputCache="[true|false]" - enable the http.sys cache on IIS6 and higher - default is true
        enableVersionHeader="[true|false]" - outputs X-AspNet-Version header with each request
    -->*/
	// theoretically, you can turn off the version header in machine.config (see above). We just make 100% sure
	pSendResponse->SetHeader(pfc, _T("X-AspNet-Version:"), '\0');

	// If you want to make fun of others, uncomment the following two lines
	// pSendResponse->SetHeader(pfc, _T("Server:"), _T("Apache/1.3.26 (Unix) mod_gzip/1.3.19.1a mod_perl/1.27 mod_ssl/2.8.10 OpenSSL/0.9.6g"));
	// pSendResponse->SetHeader(pfc, _T("X-Powered-By:"), _T("Slash 2.003000"));

	return SF_STATUS_REQ_NEXT_NOTIFICATION;
}


static HINSTANCE g_hInstance;

HINSTANCE AFXISAPI AfxGetResourceHandle()
{
	return g_hInstance;
}

BOOL WINAPI DllMain(HINSTANCE hInst, ULONG ulReason,
					LPVOID lpReserved)
{
	if (ulReason == DLL_PROCESS_ATTACH)
	{
		g_hInstance = hInst;
	}

	return TRUE;
}

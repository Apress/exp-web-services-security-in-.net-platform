#pragma once


// xaspfilter.h - Header file for your Internet Information Server
//    x-asp filter

#include "resource.h"

class CXAspxFilter : public CHttpFilter
{
public:
	CXAspxFilter();
	~CXAspxFilter();

// Overrides
	public:
	virtual BOOL GetFilterVersion(PHTTP_FILTER_VERSION pVer);
	virtual DWORD OnSendResponse(CHttpFilterContext*, PHTTP_FILTER_SEND_RESPONSE);
};

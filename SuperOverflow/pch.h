//
// pch.h
// Header for standard system include files.
//

#pragma once

#include <collection.h>
#include <msxml6.h> // IXHR2
#include <ppltasks.h>
#include "App.xaml.h"

#pragma comment(lib, "msxml6.lib")

inline void CheckHResult(HRESULT hResult)
{
    if (hResult == E_ABORT)
    {
        concurrency::cancel_current_task();
    }
    else if (FAILED(hResult))
    {
        throw Platform::Exception::CreateException(hResult);
    }
}


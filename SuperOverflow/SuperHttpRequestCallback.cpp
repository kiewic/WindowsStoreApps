#include "pch.h"
#include "InputStreamFromIsequencialStream.h"
#include "SuperHttpRequestCallback.h"

using namespace Concurrency;
using namespace SuperOverflow;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Details; // Make
using namespace std;
using namespace Windows::Storage::Streams;

SuperHttpRequestCallback::SuperHttpRequestCallback(
    IXMLHTTPRequest2* httpRequest,
    cancellation_token ct) : request(httpRequest), cancellationToken(ct)
{
    // Register a callback function that aborts the HTTP operation when 
    // the cancellation token is canceled.
    if (cancellationToken != cancellation_token::none())
    {
        registrationToken = cancellationToken.register_callback([this]() 
        {
            if (request != nullptr) 
            {
                request->Abort();
            }
        });
    }
}

SuperHttpRequestCallback::~SuperHttpRequestCallback()
{
}

//task_completion_event<IInputStream^> const& SuperHttpRequestCallback::GetCompletionEvent() const
//{
//    return completionEvent;
//}

HRESULT STDMETHODCALLTYPE SuperHttpRequestCallback::OnRedirect(
    __RPC__in_opt IXMLHTTPRequest2* xhr,
    __RPC__in_string const WCHAR* redirectUrl)
{
    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE SuperHttpRequestCallback::OnHeadersAvailable(
    __RPC__in_opt IXMLHTTPRequest2* xhr,
    DWORD statusCode,
    __RPC__in_string const WCHAR* reasonPhrase)
{
        HRESULT hr = S_OK;

        // We must not propagate exceptions back to IXHR2.
        try
        {
            this->statusCode = statusCode;
            this->reasonPhrase = reasonPhrase;
        }
        catch (std::bad_alloc&)
        {
            hr = E_OUTOFMEMORY;
        }

        return hr;
}

HRESULT STDMETHODCALLTYPE SuperHttpRequestCallback::OnDataAvailable(
    __RPC__in_opt IXMLHTTPRequest2* xhr,
    __RPC__in_opt ISequentialStream* responseStream)
{
    UNREFERENCED_PARAMETER(xhr);
    UNREFERENCED_PARAMETER(responseStream);
    return S_OK;
}

HRESULT STDMETHODCALLTYPE SuperHttpRequestCallback::OnResponseReceived(
    __RPC__in_opt IXMLHTTPRequest2* xhr,
    __RPC__in_opt ISequentialStream* responseStream)
{
    //InputStreamFromISequencialStream^ inputStreamFrom = ref new InputStreamFromISequencialStream();

    //ComPtr<IInputStream> inputStream;
    //CheckHResult(inputStreamFrom.As(&inputStream));

    // We must not propagate exceptions back to IXHR2.
    try
    {
        //completionEvent.set(inputStream.As);
    }
    catch (std::exception_ptr ex)
    {
        // TODO: Check that exceptions are really catched here.
        //completionEvent.set_exception(ex);
    }

    return S_OK;
}

HRESULT STDMETHODCALLTYPE SuperHttpRequestCallback::OnError(
    __RPC__in_opt IXMLHTTPRequest2* xhr,
    HRESULT hrError)
{
    return E_NOTIMPL;
}

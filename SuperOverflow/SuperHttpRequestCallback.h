#pragma once

#include <string>
#include <wrl.h> // RuntimeClass, RuntimeClassFlags
#include "pch.h"
#include "MainPage.g.h"

namespace SuperOverflow
{
    class SuperHttpRequestCallback :
        public Microsoft::WRL::RuntimeClass<
            Microsoft::WRL::RuntimeClassFlags<Microsoft::WRL::ClassicCom>,
            IXMLHTTPRequest2Callback,
            Microsoft::WRL::FtmBase>
    {
    public:
        SuperHttpRequestCallback(
            IXMLHTTPRequest2* httpRequest,
            Concurrency::cancellation_token ct);
        ~SuperHttpRequestCallback();

        //Concurrency::task_completion_event<
        //    Windows::Storage::Streams::IInputStream^> const& GetCompletionEvent() const;

        // IXMLHTTPRequest2Callback memebers.
        virtual HRESULT STDMETHODCALLTYPE OnRedirect(
            __RPC__in_opt IXMLHTTPRequest2* xhr,
            __RPC__in_string const WCHAR *redirectUrl);

        virtual HRESULT STDMETHODCALLTYPE OnHeadersAvailable(
            __RPC__in_opt IXMLHTTPRequest2* xhr,
            DWORD statusCode,
            __RPC__in_string const WCHAR* reasonPhrase);

        virtual HRESULT STDMETHODCALLTYPE OnDataAvailable(
            __RPC__in_opt IXMLHTTPRequest2* xhr,
            __RPC__in_opt ISequentialStream* responseStream);

        virtual HRESULT STDMETHODCALLTYPE OnResponseReceived(
            __RPC__in_opt IXMLHTTPRequest2* xhr,
            __RPC__in_opt ISequentialStream* responseStream);

        virtual HRESULT STDMETHODCALLTYPE OnError(
            __RPC__in_opt IXMLHTTPRequest2* xhr,
            HRESULT hrError);

    private:
        Concurrency::cancellation_token cancellationToken;
        Concurrency::cancellation_token_registration registrationToken;
        Microsoft::WRL::ComPtr<IXMLHTTPRequest2> request;

        // Task completion event that is set when the download operation completes.
        //Concurrency::task_completion_event<Windows::Storage::Streams::IInputStream^> completionEvent;

        int statusCode;
        std::wstring reasonPhrase;
    };
}

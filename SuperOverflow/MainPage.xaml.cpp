//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"
#include "SuperHttpRequestCallback.h"

using namespace SuperOverflow;

using namespace Concurrency;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Details; // Make
using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Security::Authentication::Web;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;
MainPage::MainPage()
{
    InitializeComponent();
}

/// <summary>
/// Invoked when this page is about to be displayed in a Frame.
/// </summary>
/// <param name="e">Event data that describes how this page was reached.  The Parameter
/// property is typically used to configure the page.</param>
void MainPage::OnNavigatedTo(NavigationEventArgs^ e)
{
    (void) e; // Unused parameter

    ComPtr<IXMLHTTPRequest2> xhr;
    CheckHResult(CoCreateInstance(CLSID_XmlHttpRequest, nullptr, CLSCTX_INPROC, IID_PPV_ARGS(&xhr)));

    cancellation_token cancellationToken = cancellation_token::none();

    // Create the callback.
    ComPtr<SuperHttpRequestCallback> superCallback = Make<SuperHttpRequestCallback>(xhr.Get(), cancellationToken);
    CheckHResult(superCallback ? S_OK : E_OUTOFMEMORY);

    // Create a request.
    CheckHResult(xhr->Open(L"GET", L"http://kiewic.com", superCallback.Get(), nullptr, nullptr, nullptr, nullptr));
    CheckHResult(xhr->Send(nullptr, 0));
}

void MainPage::DoAuthentication()
{
    String^ clientId = "1481";
    String^ redirectUri = "https://stackexchange.com/oauth/login_success";
    String^ scope = "read_inbox,no_expiry ";
    String^ authUri = "https://stackexchange.com/oauth";
    authUri += "?client_id=" + clientId + "&redirect_uri=" + redirectUri + "&scope=" + scope + "&display=popup&response_type=token";

    Uri^ startURI = ref new Uri(authUri);
    Uri^ endURI = ref new Uri(redirectUri);

    try
    {
        create_task(WebAuthenticationBroker::AuthenticateAsync(WebAuthenticationOptions::None, startURI, endURI)).then([=](WebAuthenticationResult^ result)
        {
            Uri^ responseUri;
            switch (result->ResponseStatus)
            {
            case WebAuthenticationStatus::ErrorHttp:
                OutputDebugString(("ErrorHttp: " + result->ResponseErrorDetail + "\r\n")->Data());
                break;
            case WebAuthenticationStatus::Success:
                OutputDebugString(L"Success\r\n");
                OutputDebugString((result->ResponseData + "\r\n")->Data());

                responseUri = ref new Uri(result->ResponseData);

                break;
            case WebAuthenticationStatus::UserCancel:
                OutputDebugString(L"UserCancel\r\n");
                break;
            }
        });
    }
    catch (Exception^ ex)
    {
        OutputDebugString(("Error launching WebAuth " + ex->Message)->Data());
    }
}

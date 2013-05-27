//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "ChatPage.xaml.h"
#include "MainPage.xaml.h"
#include <ppltasks.h>

using namespace TcpChat;

using namespace Concurrency;
using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Networking;
using namespace Windows::Networking::Sockets;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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
    (void) e;	// Unused parameter
}

void TcpChat::MainPage::ConnectButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    ConnectButton->IsEnabled = false;
    ConnectRing->IsActive = true;

    StreamSocket^ streamSocket = ref new StreamSocket();
    create_task(streamSocket->ConnectAsync(ref new HostName("gistanki-crazy"), "7085")).then([=](task<void> previousTask){
        try {
            previousTask.get(); // Check if there was any error.

            this->Frame->Navigate(TypeName(ChatPage::typeid), streamSocket);
        }
        catch (Exception^ ex)
        {
            OutputDebugString(ex->ToString()->Data());

            ConnectButton->IsEnabled = true;
            ConnectRing->IsActive = false;
        }
    }, cancellation_token::none(), task_continuation_context::use_current());
}

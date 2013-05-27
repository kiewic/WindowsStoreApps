//
// ChatPage.xaml.cpp
// Implementation of the ChatPage class
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
using namespace Windows::Networking::Sockets;
using namespace Windows::Storage::Streams;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Interop; // TypeName()
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

ChatPage::ChatPage() : textBoxIsEmpty(true)
{
    InitializeComponent();
}

/// <summary>
/// Invoked when this page is about to be displayed in a Frame.
/// </summary>
/// <param name="e">Event data that describes how this page was reached.  The Parameter
/// property is typically used to configure the page.</param>
void ChatPage::OnNavigatedTo(NavigationEventArgs^ e)
{
    (void) e; // Unused parameter

    streamSocket = static_cast<StreamSocket^>(e->Parameter);
    reader = ref new DataReader(streamSocket->InputStream);
    reader->InputStreamOptions = InputStreamOptions::None;
    writer = ref new DataWriter(streamSocket->OutputStream);
    writer->WriteByte(1); // Protocol version.

    ReadMessage();
}

void TcpChat::ChatPage::ReadMessage()
{
    create_task(reader->LoadAsync(4)).then([=](unsigned int bytesRead)
    {
        unsigned int messageLength = reader->ReadUInt32();
        return create_task(reader->LoadAsync(messageLength)).then([=](unsigned int bytesRead)
        {
            _ASSERT(bytesRead == messageLength);
            String^ message = reader->ReadString(bytesRead);
            OutputDebugString(message->Data());

            // TODO: Display message received.
            TextBlock^ textBlock = ref new TextBlock();
            textBlock->Text = message;
            MessagesPanel->Children->Append(textBlock);

            ReadMessage();
        }, cancellation_token::none(), task_continuation_context::use_current());
    }, cancellation_token::none(), task_continuation_context::use_current()).then([=](task<void> previousTask)
    {
        try
        {
            previousTask.get();
        }
        catch (Exception^ ex)
        {
            OutputDebugString(ex->ToString()->Data());

            const int REMOTE_HOST_CLOSED_CONNECTION = 0x80072746;
            if (ex->HResult == REMOTE_HOST_CLOSED_CONNECTION)
            {
                // Go back to the connect page.
                this->Frame->Navigate(TypeName(MainPage::typeid));
            }
        }
    });
}

void TcpChat::ChatPage::Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    // TODO: Add a lock, so only one message can be send at a time.
    MessageBox->IsEnabled = false;
    SendButton->IsEnabled = false;

    IBuffer^ buffer = StringToBuffer(MessageBox->Text);
    writer->WriteUInt32(buffer->Length);
    writer->WriteBuffer(buffer);

    create_task(writer->StoreAsync()).then([=](unsigned int bytesWritten)
    {
        // The first package could be bigger because it includes the version byte.
        _ASSERT(bytesWritten >= buffer->Length + 4);
        _ASSERT(bytesWritten <= buffer->Length + 5);
    }, cancellation_token::none(), task_continuation_context::use_arbitrary()).then([=](task<void> previousTask)
    {
        try
        {
            previousTask.get();
        }
        catch (Exception^ ex)
        {
            OutputDebugString(ex->ToString()->Data());
        }

        MessageBox->Text = "";
        MessageBox->IsEnabled = true;
        SendButton->IsEnabled = true;
    }, cancellation_token::none(), task_continuation_context::use_current());
}

IBuffer^ TcpChat::ChatPage::StringToBuffer(String^ text)
{
    DataWriter^ dataWriter = ref new DataWriter();
    dataWriter->WriteString(text);
    return dataWriter->DetachBuffer();
}

void TcpChat::ChatPage::MessageBox_GotFocus(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    if (textBoxIsEmpty)
    {
        textBoxIsEmpty = false;
        MessageBox->Foreground = ref new Media::SolidColorBrush(Colors::BlueViolet);
        MessageBox->Text = "";
    }
}

void TcpChat::ChatPage::MessageBox_LostFocus(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    if (MessageBox->Text == "")
    {
        textBoxIsEmpty = true;
        MessageBox->Foreground = ref new Media::SolidColorBrush(Colors::Gray);
        MessageBox->Text = "Type Your Message Here";
    }
}

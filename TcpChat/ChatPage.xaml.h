//
// ChatPage.xaml.h
// Declaration of the ChatPage class
//

#pragma once

#include "ChatPage.g.h"

namespace TcpChat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Windows::Foundation::Metadata::WebHostHidden]
    public ref class ChatPage sealed
    {
    public:
        ChatPage();

    protected:
        virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;

    private:
        void Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
        void MessageBox_GotFocus(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
        void MessageBox_LostFocus(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
        void ReadMessage();
        Windows::Storage::Streams::IBuffer^ StringToBuffer(Platform::String^ text);

        bool textBoxIsEmpty;
        Windows::Networking::Sockets::StreamSocket^ streamSocket;
        Windows::Storage::Streams::DataReader^ reader;
        Windows::Storage::Streams::DataWriter^ writer;
    };
}

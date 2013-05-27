//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace LyricsApp;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::System; // VirtualKey
using namespace Windows::UI::Core; // CoreWindow
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

MainPage::MainPage() : pointer1(0), pointer2(0), initialDistance(0)
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

void LyricsApp::MainPage::BiggerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    IncreaseFontSize();
}

void LyricsApp::MainPage::Smaller_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
    DecreaseFontSize();
}

void LyricsApp::MainPage::LyricText_PointerMoved(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    OutputDebugString((LyricViewer->ZoomFactor.ToString() + "\r\n")->Data());

    //unsigned int eventPointerId = e->Pointer->PointerId;
    //if (eventPointerId != pointer1 && eventPointerId != pointer2)
    //{
    //    // pointer1 <- pointer2 <- eventPointerId
    //    pointer1 = pointer2;
    //    pointer2 = eventPointerId;

    //    point1 = point2;
    //    point2 = e->GetCurrentPoint(LyricText)->Position;
    //}

    //if (pointer1 != 0 && pointer2 != 0)
    //{
    //    float newDistannce = GetDistanceBetweenPointers();
    //    if (initialDistance != 0)
    //    {
    //        if (newDistannce > initialDistance)
    //        {
    //            LyricText->FontSize += 1;
    //        }
    //        else
    //        {
    //            LyricText->FontSize -= 1;
    //        }
    //        e->Handled = true;
    //    }
    //    initialDistance = newDistannce;
    //}
}

float LyricsApp::MainPage::GetDistanceBetweenPointers()
{
    float distanceX = point1.X - point2.X;
    float distanceY = point1.Y - point2.Y;
    return sqrtf(distanceX * distanceX + distanceY * distanceY);
}

void LyricsApp::MainPage::DecreaseFontSize()
{
    if (LyricText->FontSize - 2 > 0)
    {
        LyricText->FontSize -= 2;
    }
}

void LyricsApp::MainPage::IncreaseFontSize()
{
    LyricText->FontSize += 2;
}

void LyricsApp::MainPage::Grid_KeyDown(Platform::Object^ sender, Windows::UI::Xaml::Input::KeyRoutedEventArgs^ e)
{
    CoreWindow^ coreWindow = Window::Current->CoreWindow;
    // 189 is OEMMinus
    if (static_cast<unsigned int>(e->Key) == 189)
    {
        // CTRL is down.
        if ((coreWindow->GetKeyState(VirtualKey::Control) & CoreVirtualKeyStates::Down) == CoreVirtualKeyStates::Down)
        {
            DecreaseFontSize();
            e->Handled = true;
        }
    }
    // 187 is OEMPlus
    else if (static_cast<unsigned int>(e->Key) == 187)
    {
        // CTRL is down.
        if ((coreWindow->GetKeyState(VirtualKey::Control) & CoreVirtualKeyStates::Down) == CoreVirtualKeyStates::Down)
        {
            IncreaseFontSize();
            e->Handled = true;
        }
    }
}

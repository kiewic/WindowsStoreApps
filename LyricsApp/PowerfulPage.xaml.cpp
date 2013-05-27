//
// PowerfulPage.xaml.cpp
// Implementation of the PowerfulPage class
//

#include "pch.h"
#include "PowerfulPage.xaml.h"

using namespace LyricsApp;

using namespace Platform;
using namespace Windows::Devices::Input;
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

PowerfulPage::PowerfulPage() : pointerId1(0), pointerId2(0), previousDistance(0), previousX(0)
{
    InitializeComponent();

    Window::Current->CoreWindow->KeyDown +=
        ref new TypedEventHandler<CoreWindow^, KeyEventArgs^>(this, &PowerfulPage::OnKeyDown);
}

/// <summary>
/// Invoked when this page is about to be displayed in a Frame.
/// </summary>
/// <param name="e">Event data that describes how this page was reached.  The Parameter
/// property is typically used to configure the page.</param>
void PowerfulPage::OnNavigatedTo(NavigationEventArgs^ e)
{
    (void) e;	// Unused parameter
}

void PowerfulPage::LyricPanel_ManipulationDelta(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationDeltaRoutedEventArgs^ e)
{
    OutputDebugString(L"Delta\r\n");
}

void PowerfulPage::LyricPanel_ManipulationStarted(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationStartedRoutedEventArgs^ e)
{
    OutputDebugString(L"Started\r\n");
}

void PowerfulPage::LyricPanel_PointerCanceled(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    OutputDebugString(L"Canceled\r\n");
}

void PowerfulPage::LyricPanel_PointerCaptureLost(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    OutputDebugString(L"Capture Lost\r\n");
}

void PowerfulPage::LyricPanel_PointerEntered(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    if (e->Pointer->PointerDeviceType == PointerDeviceType::Mouse)
    {
        // Sorry, mouse is not welcome.
        return;
    }

    OutputDebugString(L"Entered\r\n");
    if (pointerId1 == 0)
    {
        pointerId1 = e->Pointer->PointerId;
        point1 = e->GetCurrentPoint(this)->Position;
    }
    else if (pointerId2 == 0)
    {
        pointerId2 = e->Pointer->PointerId;
        point2 = e->GetCurrentPoint(this)->Position;
    }

    if (pointerId1 == 0 || pointerId2 == 0)
    {
        // If we do not have to pointers, we cannot calculate distance.
        return;
    }

    previousDistance = GetDistanceBetweenPointers();
}

void PowerfulPage::LyricPanel_PointerExited(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    OutputDebugString(L"Exited\r\n");
    unsigned int eventPointerId = e->Pointer->PointerId;
    if (pointerId1 == eventPointerId)
    {
        pointerId1 = 0;
    }
    else if (pointerId2 == eventPointerId)
    {
        pointerId2 = 0;
    }
}

void PowerfulPage::LyricPanel_PointerMoved(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
    float previousY = 0;
    unsigned int eventPointerId = e->Pointer->PointerId;
    if (pointerId1 == eventPointerId)
    {
        previousY = point1.Y;
        point1 = e->GetCurrentPoint(this)->Position;
    }
    else if (pointerId2 == eventPointerId)
    {
        point2 = e->GetCurrentPoint(this)->Position;
    }
    else 
    {
        // Sorry, only pointerId1 and pointerId2 are welcome.
        return;
    }

    if (pointerId1 != 0 && pointerId2 != 0)
    {
        // If we have two pointers, calculate if they are getting closer or further.
        float newDistance = GetDistanceBetweenPointers();
        float delta = newDistance - previousDistance;

        //OutputDebugString((newDistance - previousDistance).ToString()->Data());
        //OutputDebugString(L"\r\n");

        // For tiny changes do not change font size.
        if (delta > 0.5 || delta < -0.5)
        {
            // Divided by 4, otherwise the change rate is too big.
            ChangeFontSize(delta / 4);
        }

        previousDistance = newDistance;
    }
    else if (previousY != 0)
    {
        // If we only have one pointer, chheck if it is going up or down.
        float deltaY = point1.Y - previousY;

        //OutputDebugString((deltaY).ToString()->Data());
        //OutputDebugString(L"\r\n");

        if (deltaY > 0.5)
        {
            // Pointer is going down.
            ChangeTop(deltaY);
        }
        else if(deltaY < -0.5)
        {
            // Pointer is going up.
            ChangeTop(deltaY);
        }
    }
}

float PowerfulPage::GetDistanceBetweenPointers()
{
    float distanceX = point1.X - point2.X;
    float distanceY = point1.Y - point2.Y;
    return sqrtf(distanceX * distanceX + distanceY * distanceY);
}

void PowerfulPage::ChangeFontSize(float delta)
{
    if (LyricText->FontSize + delta > 10 && LyricText->FontSize + delta < 200)
    {
        LyricText->FontSize += delta;
    }
}

void PowerfulPage::ChangeTop(float delta)
{
    double currentTop = static_cast<double>(LyricPanel->GetValue(Canvas::TopProperty));
    LyricPanel->SetValue(Canvas::TopProperty, currentTop + delta);
}

void PowerfulPage::OnKeyDown(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::KeyEventArgs^ e)
{
    CoreWindow^ coreWindow = Window::Current->CoreWindow;
    // 189 is OEMMinus
    if (static_cast<unsigned int>(e->VirtualKey) == 189)
    {
        // Is CTRL down?
        if ((coreWindow->GetKeyState(VirtualKey::Control) & CoreVirtualKeyStates::Down) == CoreVirtualKeyStates::Down)
        {
            ChangeFontSize(-2);
            e->Handled = true;
        }
    }
    // 187 is OEMPlus
    else if (static_cast<unsigned int>(e->VirtualKey) == 187)
    {
        // Is CTRL down?
        if ((coreWindow->GetKeyState(VirtualKey::Control) & CoreVirtualKeyStates::Down) == CoreVirtualKeyStates::Down)
        {
            ChangeFontSize(2);
            e->Handled = true;
        }
    }
}

void LyricsApp::PowerfulPage::PowerfulCanvas_SizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e)
{
    auto width = PowerfulCanvas->ActualWidth;
    LyricPanel->Width = width;
}

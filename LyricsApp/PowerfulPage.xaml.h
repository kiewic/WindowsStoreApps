//
// PowerfulPage.xaml.h
// Declaration of the PowerfulPage class
//

#pragma once

#include "PowerfulPage.g.h"

namespace LyricsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Windows::Foundation::Metadata::WebHostHidden]
    public ref class PowerfulPage sealed
    {
    public:
        PowerfulPage();

    protected:
        virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;
    private:
        void LyricPanel_ManipulationDelta(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationDeltaRoutedEventArgs^ e);
        void LyricPanel_ManipulationStarted(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationStartedRoutedEventArgs^ e);
        void LyricPanel_PointerCanceled(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void LyricPanel_PointerCaptureLost(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void LyricPanel_PointerEntered(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void LyricPanel_PointerExited(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void LyricPanel_PointerMoved(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void OnKeyDown(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::KeyEventArgs^ args);
        float GetDistanceBetweenPointers();
        void ChangeFontSize(float delta);
        void ChangeTop(float delta);

        unsigned int pointerId1;
        unsigned int pointerId2;
        float previousDistance;
        float previousX;
        Windows::Foundation::Point point1;
        Windows::Foundation::Point point2;
        void PowerfulCanvas_SizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e);
    };
}

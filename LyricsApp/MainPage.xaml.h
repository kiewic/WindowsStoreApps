//
// MainPage.xaml.h
// Declaration of the MainPage class.
//

#pragma once

#include "MainPage.g.h"

namespace LyricsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public ref class MainPage sealed
    {
    public:
        MainPage();

    protected:
        virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;
    private:
        void BiggerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
        void Smaller_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
        void LyricText_PointerMoved(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
        void Grid_KeyDown(Platform::Object^ sender, Windows::UI::Xaml::Input::KeyRoutedEventArgs^ e);
        float GetDistanceBetweenPointers();
        void DecreaseFontSize();
        void IncreaseFontSize();

        unsigned int pointer1;
        unsigned int pointer2;
        float initialDistance;
        Windows::Foundation::Point point1;
        Windows::Foundation::Point point2;
    };
}

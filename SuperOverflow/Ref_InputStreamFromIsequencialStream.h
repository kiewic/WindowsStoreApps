#pragma once

namespace SuperOverflow
{
    private ref class InputStreamFromISequencialStream sealed : public Windows::Storage::Streams::IInputStream
    {
    public:
        InputStreamFromISequencialStream(ISequentialStream* sequencialStream)
        {
        }
        virtual ~InputStreamFromISequencialStream(void);

        virtual Windows::Foundation::IAsyncOperationWithProgress<
            Windows::Storage::Streams::IBuffer^,
            unsigned int>^ ReadAsync(
                Windows::Storage::Streams::IBuffer^ buffer,
                unsigned int count,
                Windows::Storage::Streams::InputStreamOptions options);
    };
}

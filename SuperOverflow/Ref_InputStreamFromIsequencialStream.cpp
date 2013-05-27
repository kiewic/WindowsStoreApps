#include "pch.h"
#include <ppltasks.h>
#include "InputStreamFromISequencialStream.h"

using namespace Concurrency;
using namespace SuperOverflow;
using namespace Windows::Foundation;
using namespace Windows::Storage::Streams;

//InputStreamFromISequencialStream::InputStreamFromISequencialStream(/*ISequentialStream* sequencialStream*/)
//{
//}

InputStreamFromISequencialStream::~InputStreamFromISequencialStream(void)
{
}

// IInputStream members.
IAsyncOperationWithProgress<IBuffer^, unsigned int>^ InputStreamFromISequencialStream::ReadAsync(
    IBuffer^ buffer,
    unsigned int count,
    InputStreamOptions options)
{
    return create_async([=](progress_reporter<unsigned int> progress, cancellation_token token) {
        return buffer;
    });
}

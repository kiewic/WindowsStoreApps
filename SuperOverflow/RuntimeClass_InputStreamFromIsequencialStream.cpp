#include "pch.h"
#include <ppltasks.h>
#include "InputStreamFromIsequencialStream.h"

using namespace Concurrency;
using namespace Windows::Foundation;
using namespace Windows::Storage::Streams;

InputStreamFromIsequencialStream::InputStreamFromIsequencialStream(ISequentialStream* sequencialStream)
{
}

InputStreamFromIsequencialStream::~InputStreamFromIsequencialStream(void)
{
}

HRESULT STDMETHODCALLTYPE InputStreamFromIsequencialStream::ReadAsync(
    ABI::Windows::Storage::Streams::IBuffer* buffer,
    UINT32 count,
    ABI::Windows::Storage::Streams::InputStreamOptions options,
    ABI::Windows::Foundation::IAsyncOperationWithProgress<ABI::Windows::Storage::Streams::IBuffer*, UINT32>** operation)
{
    *operation = create_async([=](progress_reporter<unsigned int> progress, cancellation_token token) {
        return (ABI::Windows::Storage::Streams::IBuffer*)nullptr;
    });
    return E_NOTIMPL;
}

//// IInputStream members.
//IAsyncOperationWithProgress<IBuffer^, unsigned int>^ InputStreamFromIsequencialStream::ReadAsync(
//    IBuffer^ buffer,
//    unsigned int count,
//    InputStreamOptions options)
//{
//    return create_async([=](progress_reporter<unsigned int> progress, cancellation_token token) {
//        return buffer;
//    });
//}

#pragma once
#include <windows.storage.streams.h>
#include <wrl\implements.h>

class InputStreamFromIsequencialStream :
    public Microsoft::WRL::RuntimeClass<ABI::Windows::Storage::Streams::IInputStream, Microsoft::WRL::FtmBase>
{
    InspectableClass(L"SuperOverflow.InputStreamFromIsequencialStream", BaseTrust);

public:
    InputStreamFromIsequencialStream(ISequentialStream* sequencialStream);
    virtual ~InputStreamFromIsequencialStream(void);

    virtual HRESULT STDMETHODCALLTYPE ReadAsync(
        ABI::Windows::Storage::Streams::IBuffer *buffer,
        UINT32 count,
        ABI::Windows::Storage::Streams::InputStreamOptions options,
        ABI::Windows::Foundation::IAsyncOperationWithProgress<ABI::Windows::Storage::Streams::IBuffer*, UINT32>** operation);
};

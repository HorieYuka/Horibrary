#include "pch.h"

#include "Wrapper.h"

Wrapper::LibraryVISA::LibraryVISA()
{
	Lib = new Library();
};

char* Wrapper::LibraryVISA::GetDeviceInfo(int DeviceIdx)
{
	return Lib->GetDeviceInfo(DeviceIdx);
}

int Wrapper::LibraryVISA::GetDeviceCount()
{
	return Lib->GetDeviceCount();
}


bool Wrapper::LibraryVISA::FindResource()
{
	return Lib->FindResource();
}

bool Wrapper::LibraryVISA::OpenSession(int DeviceIdx)
{
	return Lib->OpenSession(DeviceIdx);
}

bool Wrapper::LibraryVISA::CloseSession(int DeviceIdx)
{
	return Lib->CloseSession(DeviceIdx);
}
bool Wrapper::LibraryVISA::DisposeManager()
{
	return Lib->DisposeManager();
}


bool Wrapper::LibraryVISA::IOWrite(int DeviceIdx, char* Str, unsigned int StrLen)
{
	return Lib->IOWrite(DeviceIdx, Str,StrLen);
}
unsigned char* Wrapper::LibraryVISA::IORead(int DeviceIdx, char* Str, unsigned int StrLen)
{
	return Lib->IORead(DeviceIdx, Str, StrLen);
}

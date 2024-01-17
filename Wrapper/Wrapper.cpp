#include "pch.h"

#include "Wrapper.h"

Wrapper::LibraryVISA::LibraryVISA()
{
	Lib = new Library();
};

char** Wrapper::LibraryVISA::GetDescList()
{
	return Lib->GetDescList();
}

bool Wrapper::LibraryVISA::FindResource()
{
	return Lib->FindResource();
}

bool Wrapper::LibraryVISA::OpenSession(int Idx)
{
	return Lib->OpenSession(Idx);
}

bool Wrapper::LibraryVISA::CloseSession(int Idx)
{
	return Lib->CloseSession(Idx);
}
bool Wrapper::LibraryVISA::DisposeManager()
{
	return Lib->DisposeManager();
}
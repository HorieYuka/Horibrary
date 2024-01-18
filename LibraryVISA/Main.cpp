// Test library

#include "LibraryVISA.h"

char* InstDescriptor;

int main() 
{
	Library* Lib = new Library();
	Lib->FindResource();

	return 0;
}
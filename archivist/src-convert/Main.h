//-----------------------------------------------------------------------------
// Copyright (C) 2004 Sebastian Pech
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//  Main.h
//  http://www.spech.de
//  - Main File -
//-----------------------------------------------------------------------------

#ifndef __MAINH__
#define __MAINH__

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#if defined(WIN32)
	#define LF3DPLATFORM_WINDOWS
	#define WIN32_LEAN_AND_MEAN
	#include <windows.h>
#endif

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
using namespace std;

//-----------------------------------------------------------------------------
// Functions
//-----------------------------------------------------------------------------
void PrintHelp();

//-----------------------------------------------------------------------------
// Structure for cards
//-----------------------------------------------------------------------------
struct strCard
{
	string 	Name;
	int		Location;
	string	Cost;
	string	Expansion;
	string	ExpansionShrt;
	string 	Type;
	string	PowTgh;
	string 	Text;
	string	Flavor;
	
	// functor to sort several Elements in a container (vector)
	struct Less : public std::binary_function <const strCard&, const strCard&, bool> {
		bool operator() (const strCard& A, const strCard& B) {
			return (A.Name < B.Name);
		}
	};
};

//-----------------------------------------------------------------------------
// Converter
//-----------------------------------------------------------------------------
class CConverter
{
	private:
		vector<strCard>		prv_Cards;
		map<string, string>	prv_Expansions;
		
		string 	ReadLine(ifstream &Stream);
		int		ReadInt(FILE* File);
		int 	ReadInt8(FILE* File);
		string 	ReadString(FILE* File);
		void	CleanString(string &String);
		void CConverter::StringReplace(string &String, string Find, string Replace);
		void	CostConvert(string &String);
		
	public:
		int  ImportOracle(string Oracle);
		int  ImportApprentice();
		int  ExportArchivist(string Out);
};

#endif

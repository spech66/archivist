//-----------------------------------------------------------------------------
//  CardInfo.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Manage all cards -
//-----------------------------------------------------------------------------

#ifndef __CARDINFOH__
#define __CARDINFOH__

//-----------------------------------------------------------------------------
// Structure for cards
//-----------------------------------------------------------------------------
struct strCard
{
	string 			Name;
	string			Cost;
	vector<string>	Expansion;
	vector<string>	ExpansionShrt;
	string 			Type;
	string			PowTgh;
	string 			Text;
	string			Flavor;
};

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class CCardInfo
{
	private:
		FILE* 				prv_File;		
		vector<strCard> 	prv_Cards;
		vector<string> 		prv_Expansions;
		vector<string> 		prv_Types;
		
		int	ReadInt();
		int ReadInt8();
		string ReadString();
			
	public:
		CCardInfo();
		vector<strCard*>	GetAllCards();
		vector<string*>		GetAllExpansions();
		vector<string*>		GetAllTypes();
		strCard*			GetCardByName(string Name);
};

#endif

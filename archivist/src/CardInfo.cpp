//-----------------------------------------------------------------------------
//  CardInfo.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Manage all cards -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

CCardInfo::CCardInfo()
{
	ifstream File("data/Cards.acf");
	if(!File.is_open())
	{
		wxMessageDialog Message(NULL, "Cards.acf not found. Aborting!", "Error", wxOK | wxICON_ERROR);
		Message.ShowModal();
		exit(1);
	}

	while(!File.eof())
	{
		strCard TempCard;
		string Temp = ReadLine(File);
		while("" != Temp)
		{
			string::size_type Pos;
			Pos = Temp.find("=");
			if(string::npos != Pos)
			{
				string Section = Temp.substr(0, Pos);
				string Text = Temp.substr(Pos+1, Temp.size());
				if("Name" == Section)	TempCard.Name = Text;
				if("Cost" == Section)	TempCard.Cost = Text;
				if("Type" == Section)	TempCard.Type = Text;
				if("PowTgh" == Section)	TempCard.PowTgh = Text;
				if("Text" == Section)
				{
					TempCard.Text = Text;
					string::size_type TextPos;
					while(string::npos != (TextPos = TempCard.Text.find("#")))
						TempCard.Text.replace(TextPos, 1, "\n");
				}
				if("Flavor" == Section)
				{
					TempCard.Flavor = Text;
					string::size_type FlavPos;
					while(string::npos != (FlavPos = TempCard.Flavor.find("#")))
						TempCard.Flavor.replace(FlavPos, 1, "\n");
				}
				if("Expansion" == Section) SplitString(Text, "#", &TempCard.Expansion);
				if("ExpansionShrt" == Section) SplitString(Text, "#", &TempCard.ExpansionShrt);
			}
			Temp = ReadLine(File);
		}
		prv_Cards.push_back(TempCard);
	}	
	
	for(int i = 0; i < prv_Cards.size(); i++)
	{
		prv_Types.push_back(prv_Cards[i].Type);
		for(int j = 0; j < prv_Cards[i].Expansion.size(); j++)
			prv_Expansions.push_back(prv_Cards[i].Expansion[j]);
	}
	sort(prv_Types.begin(), prv_Types.end());
	prv_Types.erase(unique(prv_Types.begin(), prv_Types.end()), prv_Types.end());
	sort(prv_Expansions.begin(), prv_Expansions.end());
	prv_Expansions.erase(unique(prv_Expansions.begin(), prv_Expansions.end()), prv_Expansions.end());
}

vector<strCard*> CCardInfo::GetAllCards()
{
	vector<strCard*> Temp;
	for(int i = 0; i < prv_Cards.size(); i++)
	{
		Temp.push_back(&prv_Cards[i]);
	}
	return Temp;
}

vector<string*> CCardInfo::GetAllExpansions()
{
	vector<string*> Temp;
	for(int i = 0; i < prv_Expansions.size(); i++)
	{
		if("" != prv_Expansions[i])
			Temp.push_back(&prv_Expansions[i]);
	}
	return Temp;
}

vector<string*> CCardInfo::GetAllTypes()
{
	vector<string*> Temp;
	for(int i = 0; i < prv_Types.size(); i++)
		if("" != prv_Types[i])
			Temp.push_back(&prv_Types[i]);
	return Temp;
}

strCard* CCardInfo::GetCardByName(string Name)
{
	for(int i = 0; i < prv_Cards.size(); i++)
	{
		if(Name == prv_Cards[i].Name)
			return &prv_Cards[i];
	}
	
	return NULL;
}

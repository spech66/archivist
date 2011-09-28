//-----------------------------------------------------------------------------
//  Main.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "Lightfire3D Engine".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Main File -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

int main(int argc, char* argv[])
{
	if(argc < 3)
	{
		PrintHelp();
		return 0;
	} else {
		CConverter Converter;
		
		string Type = argv[1];
		if("-o" == Type)
		{
			cout << "Oracle import" << endl;
			if(0 == Converter.ImportOracle(argv[2]))
				Converter.ExportArchivist(argv[3]);
		} else if("-a" == Type) {
			cout << "Apprentice import" << endl;
			if(0 == Converter.ImportApprentice())
				Converter.ExportArchivist(argv[2]);
		} else {
			PrintHelp();
			return 0;
		}
	}
	
	return 0;
}

void PrintHelp()
{
	cout << "Archivist - File converter" << endl;
	cout << endl;
	cout << "Usage: oracleconv -o OracleFile OutputFile" << endl;
	cout << "Reads Wizards OracleFile and saves it as OutputFile.acf" << endl;
	cout << endl;
	cout << "Usage: oracleconv -a OutputFile" << endl;
	cout << "Reads Apprentice CardInfo.dat, Expan.dat and saves it as OutputFile.acf." << endl;
}

int CConverter::ImportOracle(string Oracle)
{
	ifstream OracleFile(Oracle.c_str());
	if(!OracleFile.is_open())
	{
		cout << "Error: could not read >" << Oracle << "<" << endl;
		return 1;
	}

	while(!OracleFile.eof())
	{
		string Temp;
		
		Temp = ReadLine(OracleFile);
		//Ignore empty lines
		if("\r" == Temp)
			continue;
		
		strCard Card;
		//Card name is alwasy first
		Card.Name = Temp;
		
		Temp = ReadLine(OracleFile);
		//Costs or not?
		if('{' == Temp[0])
		{
			//If so save it and read next line
			Card.Cost = Temp;
			Temp = ReadLine(OracleFile);
		}
		
		//Now save the type
		Card.Type = Temp;
		
		Temp = ReadLine(OracleFile);
		//Creatures have Pow/Tgh
		if(string::npos != Card.Type.find("Creature") &&
			string::npos == Card.Type.find("Enchant"))
		{
			Card.PowTgh = Temp;
			Temp = ReadLine(OracleFile);
		}
		
		Card.Text = Temp + "\n";
		while("\r" != (Temp = ReadLine(OracleFile)) && !OracleFile.eof())
			Card.Text += Temp + "\n";
		Card.Text = Card.Text.substr(0, Card.Text.size()-1);		
		
		CleanString(Card.Cost);
		CleanString(Card.Type);
		CleanString(Card.PowTgh);
		CleanString(Card.Name);	
		prv_Cards.push_back(Card);
	}
	
	OracleFile.close();
	
	return 0;
}

int CConverter::ImportApprentice()
{
	ifstream FileExpan("Expan.dat");
	if(!FileExpan.is_open())
	{
		cout << "Error: could not read >Expan.dat<" << endl;
		return 1;
	}
	
	while(!FileExpan.eof())
	{
		char cTemp[256];
		string Temp = ReadLine(FileExpan);
		if('-' == Temp[0])
			break;
		prv_Expansions[Temp.substr(0, 2)] = Temp.substr(3, Temp.size()-3);
	}
	FileExpan.close();
	
	int Cards;
	FILE* File = fopen("CardInfo.dat", "r");
	if(NULL == File)
	{
		cout << "Error: could not read >CardInfo.dat<" << endl;
		return 1;
	}
	
	fseek(File, 0l, SEEK_END);
	if(ftell(File) == 0)
	{
		fclose(File);
		cout << "Error: Empty CardInfo.dat!" << endl;
		return 1;
	}
	fseek(File, 0l, SEEK_SET);
	
	int Loc = ReadInt(File);
	if(fseek(File, Loc, SEEK_SET) < 0)
	{
		cout << "Error: Seek CardInfo.dat" << endl;
		fclose(File);
		return 1;
	}
	
	Cards = ReadInt(File);
	int i = 0;
	for(i = 0; i < Cards; i++)
	{
		strCard TempCard;
		TempCard.Name = ReadString(File);
		TempCard.Location = ReadInt(File);
		ReadInt8(File); //8bit Color => very stupid calculated

		string Expan = ReadString(File);	
		string::size_type pos = -1; //Evil Hack to handle first entry
		do
		{
			TempCard.Expansion += (prv_Expansions[Expan.substr(pos+1, 2)]) + "#";
			TempCard.ExpansionShrt += Expan.substr(pos+1, 2) + "#";
		} while(string::npos != (pos = Expan.find(",", pos+1)));
		while(string::npos != (pos = TempCard.Expansion.find("\r")))
			TempCard.Expansion.replace(pos, 1, "");
		TempCard.Expansion = TempCard.Expansion.substr(0, TempCard.Expansion.size()-1);
		TempCard.ExpansionShrt = TempCard.ExpansionShrt.substr(0, TempCard.ExpansionShrt.size()-1);

		ReadInt8(File);
		ReadInt8(File);
		prv_Cards.push_back(TempCard);
	}
	
	for(i = 0; i < prv_Cards.size(); i++)
	{
		if(fseek(File, prv_Cards[i].Location, SEEK_SET) < 0)
			continue;
		prv_Cards[i].Type 	= ReadString(File);
		prv_Cards[i].Cost 	= ReadString(File);
		prv_Cards[i].PowTgh = ReadString(File);
		prv_Cards[i].Text 	= ReadString(File);
		prv_Cards[i].Flavor = ReadString(File);
		CostConvert(prv_Cards[i].Cost);
	}
	fclose(File);
	
	return 0;
}

int CConverter::ExportArchivist(string Out)
{
	sort(prv_Cards.begin(), prv_Cards.end(), strCard::Less());

	Out += ".acf";
	ofstream OutputFile(Out.c_str());
	if(!OutputFile.is_open())
	{
		cout << "Error: could not write >" << Out << "<" << endl;
		return 1;
	}
	
	for(int i = 0; i < prv_Cards.size(); i++)
	{
		cout << "Writing "<< prv_Cards[i].Name << endl;
		
		string::size_type pos = 0;
		CleanString(prv_Cards[i].Text);
		CleanString(prv_Cards[i].Flavor);
		while(string::npos != (pos = prv_Cards[i].Text.find("\n")))
			prv_Cards[i].Text.replace(pos, 1, "#");
		while(string::npos != (pos = prv_Cards[i].Flavor.find("\n")))
			prv_Cards[i].Flavor.replace(pos, 1, "#");
			
		while(string::npos != (pos = prv_Cards[i].Type.find("--")))
			prv_Cards[i].Type.replace(pos, 2, "-");

		StringReplace(prv_Cards[i].Name, "Æ", "AE");
		StringReplace(prv_Cards[i].Text, "Æ", "AE");
			
		if("" != prv_Cards[i].Name)
			OutputFile << "Name=" << prv_Cards[i].Name << endl;
		if("" != prv_Cards[i].Cost)
			OutputFile << "Cost=" << prv_Cards[i].Cost << endl;
		if("" != prv_Cards[i].ExpansionShrt)
			OutputFile << "ExpansionShrt=" << prv_Cards[i].ExpansionShrt << endl;
		if("" != prv_Cards[i].Expansion)
			OutputFile << "Expansion=" << prv_Cards[i].Expansion << endl;
		if("" != prv_Cards[i].Type)
			OutputFile << "Type=" << prv_Cards[i].Type << endl;
		if("" != prv_Cards[i].PowTgh)
			OutputFile << "PowTgh=" << prv_Cards[i].PowTgh << endl;
		if("" != prv_Cards[i].Text)
			OutputFile << "Text=" << prv_Cards[i].Text << endl;
		if("" != prv_Cards[i].Flavor)
			OutputFile << "Flavor=" << prv_Cards[i].Flavor << endl;
		
		OutputFile << endl;
	}
	
	OutputFile.close();
	
	return 0;
}

int	CConverter::ReadInt(FILE* File)
{
	int i;
	fread(&i, sizeof(i), 1, File);
	return i;
}

int CConverter::ReadInt8(FILE* File)
{
	unsigned char i;
	fread(&i, sizeof(i), 1, File);
	return i;
}

string CConverter::ReadString(FILE* File)
{
	unsigned short Len;
	char *Data;
	
	fread(&Len, sizeof(Len), 1, File);
	if(0 == Len)
		return "";
	Data = new char[Len+1];
	fread(Data, 1, Len, File);
	while((Len > 0) && isspace(Data[Len-1]))
		Len--;
	Data[Len] = 0;
	
	string Ret = Data;
	delete Data;
	return Ret;
}

string CConverter::ReadLine(ifstream &Stream)
{
	char TempLine[2048];
	Stream.getline(TempLine, 2048);
	string Temp = TempLine;
	return Temp;
}

void CConverter::CleanString(string &String)
{
	string::size_type Pos = 0;
	while(string::npos != (Pos = String.find("\r")))
		String.replace(Pos, 1, "");
}

void CConverter::StringReplace(string &String, string Find, string Replace)
{
	string::size_type Pos = 0;
	while(string::npos != (Pos = String.find(Find)))
		String.replace(Pos, Find.size(), Replace);
}

void CConverter::CostConvert(string &String)
{
	string Temp = String;
	string End;
	char Color[] = {'X', 'W', 'U', 'R', 'B', 'G'};
	for(int j = 0; j < 6; j++)
	{
		for(int i = 0; i < Temp.size(); i++)
		{
			if(0 == strncasecmp(&Color[j], &Temp[i], 1))
			{
				End += string("{") + Color[j] + "}";
				Temp[i] = ' ';
			}
		}
	}
	
	string::size_type pos;
	while(string::npos != (pos = Temp.find(" ")))
		Temp.replace(pos, 1, "");
	
	if("" != Temp)
		End = string("{") + Temp + string("}") + End; 

	String = End;		
}

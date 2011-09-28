//-----------------------------------------------------------------------------
//  Helper.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Frequently used functions -
//-----------------------------------------------------------------------------

#ifndef __HELPERH__
#define __HELPERH__

string::size_type StringNocaseFind(string str, string fnd);
void ListCtrlAmountChange(wxListCtrl* ListCtrl, long Index, int Column, bool Decrease = false, unsigned int Amount = 1);
string LTrim(string String);
string ReadLine(ifstream &Stream);
int SplitString(string Data, string Delimeter, vector<string> *SplitData);
wxColour CardToWxColour(string CardColour, string CardType);
bool DownloadImage(string Server, string Url, string Dest);

#endif

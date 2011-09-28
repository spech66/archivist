//-----------------------------------------------------------------------------
//  Helper.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Frequently used functions -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

string::size_type StringNocaseFind(string str, string fnd)
{
	transform(str.begin(), str.end(), str.begin(), (int(*)(int))toupper);
	transform(fnd.begin(), fnd.end(), fnd.begin(), (int(*)(int))toupper);
	return str.find(fnd);
}

void ListCtrlAmountChange(wxListCtrl* ListCtrl, long Index, int Column, bool Decrease, unsigned int Amount)
{			
	wxListItem Item;
	Item.SetMask(wxLIST_MASK_TEXT);
	Item.SetId(Index);
	Item.SetColumn(Column);
	ListCtrl->GetItem(Item);
	int ItemAmount = atoi(Item.GetText().c_str());
	if(Decrease)
		ItemAmount -= Amount;
	else
		ItemAmount += Amount;
	char c[12];
	snprintf(c, 12, "%i", ItemAmount);
	ListCtrl->SetItem(Index, Column, c);
}

string LTrim(string String)
{
	for(int i = 0; i < String.size(); i++)
	{
		if(' ' != String[i] && '\t' != String[i])
		{
			return String.substr(i, String.size()-i);
		}
	}
	
	return String;
}

string ReadLine(ifstream &Stream)
{
	char TempLine[2048];
	Stream.getline(TempLine, 2048);
	string Temp = TempLine;
	return Temp;
}

int SplitString(string Data, string Delimeter, vector<string> *SplitData)
{
	string TempData;
	unsigned int TempPos = 0;
	int TempFoundPos = 0;
	
	SplitData->clear();
	
	while(true)
	{
		if(-1 == (TempFoundPos = Data.find(Delimeter, TempPos)))
		{
			TempData = Data.substr(TempPos, Data.length() - TempPos);
			SplitData->push_back(TempData);
			break;
		}
		TempData = Data.substr(TempPos, TempFoundPos - TempPos);
		TempPos = TempFoundPos + 1;
		SplitData->push_back(TempData);
	}
	
	return SplitData->size();
}

wxColour CardToWxColour(string CardColour, string CardType)
{
	bool W = false, U = false, R = false, B = false, G = false;
	bool L = false, A = false;
	if(string::npos != CardColour.find("W")) W = true;
	if(string::npos != CardColour.find("U")) U = true;
	if(string::npos != CardColour.find("R")) R = true;
	if(string::npos != CardColour.find("B")) B = true;
	if(string::npos != CardColour.find("G")) G = true;
	if(!W && !U && !R && !B && !G)
	{
		if(string::npos != CardType.find("Land"))
			L = true;
		else
			A = true;
	}
	
	if(A)	return wxColour(149, 149, 149);
	if(L)	return wxColour(162, 103, 11);
	if(W && !U && !R && !B && !G)	return wxColour(255, 255, 255);
	if(!W && U && !R && !B && !G)	return wxColour(159, 177, 207);
	if(!W && !U && R && !B && !G)	return wxColour(228, 168, 175);
	if(!W && !U && !R && B && !G)	return wxColour(88, 88, 88);
	if(!W && !U && !R && !B && G)	return wxColour(154, 194, 163);
	
	return wxColour(207, 221, 51);
}

bool DownloadImage(string Server, string Url, string Dest)
{
	#if defined(WIN32)
		WSADATA Data;
		if(0 != WSAStartup(MAKEWORD(2, 0), &Data))
			cout << "Error during WSA Startup " << WSAGetLastError() << endl;
	#endif
	
	int Socket = socket(AF_INET, SOCK_STREAM, 0);
	if(-1 == Socket)
	{
		cout << "Error: Could not create Socket for Connect" << endl;
		return false;
	}
	
	struct sockaddr_in ServerAddr;
	memset(&ServerAddr, 0, sizeof(ServerAddr));
	ServerAddr.sin_family = AF_INET;
	ServerAddr.sin_port = htons(80);
	
	if(INADDR_NONE != inet_addr(Server.c_str()))
	{
		ServerAddr.sin_addr.s_addr = inet_addr(Server.c_str());
	} else {
		struct hostent* Host = gethostbyname(Server.c_str());
		if(NULL != Host)
		{
			memcpy(&(ServerAddr.sin_addr), Host->h_addr_list[0], 4);
			Server = Host->h_addr_list[0];
		} else {
			cout << "Error: Could not resolve Hostname " << Server.c_str() << endl;
			return false;
		}
	}
	
	if(-1 == connect(Socket, (struct sockaddr*)&ServerAddr, sizeof(struct sockaddr)))
	{
		cout << "Error: Connect to Server - " << strerror(errno) << endl;
		return false;
	}
	
	
	string Data = "GET " + Url + " HTTP/1.0\r\n\r\n";
	send(Socket, (char*)(Data.c_str()), Data.length(), 0);
	
	int Ret = -2;
	string Incoming = "";
	while(-1 != Ret && 0 != Ret)
	{
		char RecvData[1024];
		Ret = recv(Socket, reinterpret_cast<char*>(RecvData), 1023, 0);
		for(int i = 0; i < Ret; i++)
			Incoming += RecvData[i];
	}

	if(string::npos == Incoming.find("HTTP/1.1 200 OK"))
	{
		cout << "No HTTP/1.1 200 OK received" << endl;
		return false;
	}

	string::size_type pos = Incoming.find("\r\n\r\n", 0);
	
	ofstream f(Dest.c_str());
	f << Incoming.substr(pos+4, Incoming.size() - pos - 4);
	f.close();
	
	#if defined(WIN32)
	if(-1 != Socket)
		closesocket(Socket);
	if(SOCKET_ERROR == WSACleanup())
		cout << "Error: WSACleanup() fails" << endl;
	#else
	if(-1 != Socket)
		close(Socket);
	#endif
}

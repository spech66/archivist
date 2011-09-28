//-----------------------------------------------------------------------------
//  WndLibrary.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The library page -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

CWndLibrary::CWndLibrary(MagicFrame* Frame, wxNotebook* parent): 
	wxNotebookPage(parent, -1)
{
	prv_Frame = Frame;
	
	wxStaticBox *Box = new wxStaticBox(this, -1, "Library", wxPoint(1, 1), wxSize(408, 640));
	ListBox = new wxListCtrl(this, WL_LISTBOX, wxPoint(4, 20), wxSize(400, 610),  wxLC_REPORT|wxLC_HRULES);
	
	//Assign Values	
	
	ListBox->InsertColumn(0, "Name", wxLIST_FORMAT_LEFT, 320);
	ListBox->InsertColumn(1, "Amount", wxLIST_FORMAT_LEFT, 60);
	
	ifstream LibFile("data/Library.dat");
	if(LibFile.is_open())
	{
		while(!LibFile.eof())
		{
			char Amount[16];
			char Name[256];
			LibFile >> Amount;
			LibFile.getline(Name, 256);
			string NameStr = Name;
			if("" != NameStr && 0 < atoi(Amount))
			{
				int Index = ListBox->GetItemCount();
				ListBox->InsertItem(Index, NameStr.substr(1, NameStr.size()-1).c_str());
				ListBox->SetItem(Index, 1, Amount);
			}
		}
	}
	LibFile.close();
	
	Menu = new wxMenu("Amount");
	Menu->Append(WL_MENUDEL, "Delete");
	Menu->Append(WL_MENUI1, "+1");
	Menu->Append(WL_MENUD1, "-1");
	Menu->Append(WL_MENUI10, "+10");
	Menu->Append(WL_MENUD10, "-10");
	
	this->Connect(WL_MENUDEL, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndLibrary::OnDelete);	
	this->Connect(WL_MENUI1, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndLibrary::OnAmountI1);
	this->Connect(WL_MENUD1, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndLibrary::OnAmountD1);
	this->Connect(WL_MENUI10, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndLibrary::OnAmountI10);
	this->Connect(WL_MENUD10, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndLibrary::OnAmountD10);
		
	this->Connect(WL_LISTBOX, wxEVT_COMMAND_LIST_ITEM_SELECTED,
				 (wxObjectEventFunction)(wxEventFunction)(wxListEventFunction)
				 &CWndLibrary::OnSelect);
}

CWndLibrary::~CWndLibrary()
{
	ofstream LibFile("data/Library.dat");
	long Items = ListBox->GetItemCount();
	if(LibFile.is_open())
	{
		for(int i = 0; i < Items; i++)
		{
			wxListItem Item;
			Item.SetMask(wxLIST_MASK_TEXT);
			Item.SetId(i);
			Item.SetColumn(1);
			ListBox->GetItem(Item);
			LibFile << Item.GetText().c_str() << " ";
			Item.SetColumn(0);
			ListBox->GetItem(Item);
			LibFile << Item.GetText().c_str() << endl;
		}
	}
	LibFile.close();
}

void CWndLibrary::OnSelect(wxListEvent &Event)
{
	this->PopupMenu(Menu, Event.GetPoint());
	prv_Frame->WndCardInfo->ShowCard(Event.GetText().c_str());
}

void CWndLibrary::OnDelete(wxCommandEvent& WXUNUSED(event))
{
	ListBox->DeleteItem(ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED));
}

void CWndLibrary::OnAmountI1(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1);
}

void CWndLibrary::OnAmountD1(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, true);
}

void CWndLibrary::OnAmountI10(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, false, 10);
}

void CWndLibrary::OnAmountD10(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, true, 10);
}

void CWndLibrary::AddCard(string Name)
{
	long FindIndex = -1;
	if(-1 != (FindIndex = ListBox->FindItem(FindIndex, Name.c_str())))
	{
		ListCtrlAmountChange(ListBox, FindIndex, 1);
		return;
	}
	
	int Index = ListBox->GetItemCount();
	ListBox->InsertItem(Index, Name.c_str());
	ListBox->SetItem(Index, 1, "1");
}

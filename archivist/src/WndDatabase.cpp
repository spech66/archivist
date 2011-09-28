//-----------------------------------------------------------------------------
//  WndDatabase.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The database page -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

CWndDatabase::CWndDatabase(MagicFrame* Frame, wxNotebook* parent):
	wxNotebookPage(parent, -1)
{
	prv_Frame = Frame;
	
	wxStaticBox *Box = new wxStaticBox(this, -1, "Cards", wxPoint(1, 1), wxSize(408, 640));
	ListBox = new wxListCtrl(this, WD_LISTBOX, wxPoint(4, 20), wxSize(400, 610), wxLC_REPORT|wxLC_HRULES);
	
	wxStaticBox *Box2 = new wxStaticBox(this, -1, "Image", wxPoint(420, 1), wxSize(220, 340));
	CardWindow = new wxNewBitmap(this, WD_STATICBMP, wxBitmap(wxT("img/none.jpg"), wxBITMAP_TYPE_ANY), wxPoint(424, 20), wxSize(210, 300));
	
	wxStaticBox *Box3 = new wxStaticBox(this, -1, "Information", wxPoint(420, 342), wxSize(220, 300));
	CardName = new wxTextCtrl(this, -1, "", wxPoint(424, 362), wxSize(210, 20), wxTE_READONLY);
	CardType = new wxTextCtrl(this, -1, "", wxPoint(424, 382), wxSize(210, 20), wxTE_READONLY);
	CardText = new wxTextCtrl(this, -1, "", wxPoint(424, 412), wxSize(210, 80), wxTE_READONLY|wxTE_MULTILINE);
	CardFlavor = new wxTextCtrl(this, -1, "", wxPoint(424, 492), wxSize(210, 80), wxTE_READONLY|wxTE_MULTILINE);
	CardPowTgh = new wxTextCtrl(this, -1, "", wxPoint(424, 572), wxSize(210, 20), wxTE_READONLY);
	CardExpansion = new wxListBox(this, -1, wxPoint(424, 592), wxSize(210, 40));
	
	wxStaticBox *Box4 = new wxStaticBox(this, -1, "Search", wxPoint(650, 1), wxSize(368, 640));
	wxStaticText *Descr1 = new wxStaticText(this, -1, "Name:", wxPoint(654, 20));
	SearchName = new wxTextCtrl(this, -1, "", wxPoint(754, 20), wxSize(260, 20));
	SearchOk = new wxButton(this, WD_SEARCHOK, "Search", wxPoint(654, 590), wxSize(200, 40));
	SearchReset = new wxButton(this, WD_SEARCHRESET, "Reset", wxPoint(854, 590), wxSize(158, 40));
	wxString SearchManaValues[] = {"May", "Must", "Must not"};
	wxStaticBitmap* SearchMana1 = new wxStaticBitmap(this, -1, wxBitmap(wxT("img/u.gif"), wxBITMAP_TYPE_ANY), wxPoint(658, 50));
	SearchManaU = new wxComboBox(this, -1, SearchManaValues[0], wxPoint(674, 50), wxSize(100, 20), 3, SearchManaValues, wxCB_DROPDOWN|wxCB_READONLY);
	wxStaticBitmap* SearchMana2 = new wxStaticBitmap(this, -1, wxBitmap(wxT("img/b.gif"), wxBITMAP_TYPE_ANY), wxPoint(778, 50));
	SearchManaB = new wxComboBox(this, -1, SearchManaValues[0], wxPoint(794, 50), wxSize(100, 20), 3, SearchManaValues, wxCB_DROPDOWN|wxCB_READONLY);
	wxStaticBitmap* SearchMana3 = new wxStaticBitmap(this, -1, wxBitmap(wxT("img/w.gif"), wxBITMAP_TYPE_ANY), wxPoint(898, 50));
	SearchManaW = new wxComboBox(this, -1, SearchManaValues[0], wxPoint(914, 50), wxSize(100, 20), 3, SearchManaValues, wxCB_DROPDOWN|wxCB_READONLY);
	wxStaticBitmap* SearchMana4 = new wxStaticBitmap(this, -1, wxBitmap(wxT("img/r.gif"), wxBITMAP_TYPE_ANY), wxPoint(658, 80));
	SearchManaR = new wxComboBox(this, -1, SearchManaValues[0], wxPoint(674, 80), wxSize(100, 20), 3, SearchManaValues, wxCB_DROPDOWN|wxCB_READONLY);
	wxStaticBitmap* SearchMana5 = new wxStaticBitmap(this, -1, wxBitmap(wxT("img/g.gif"), wxBITMAP_TYPE_ANY), wxPoint(778, 80));
	SearchManaG = new wxComboBox(this, -1, SearchManaValues[0], wxPoint(794, 80), wxSize(100, 20), 3, SearchManaValues, wxCB_DROPDOWN|wxCB_READONLY);
	wxStaticText *Descr2 = new wxStaticText(this, -1, "Text:", wxPoint(654, 110));
	SearchText= new wxTextCtrl(this, -1, "", wxPoint(754, 110), wxSize(260, 20));
	wxStaticText *Descr3 = new wxStaticText(this, -1, "Flavor:", wxPoint(654, 130));
	SearchFlavor= new wxTextCtrl(this, -1, "", wxPoint(754, 130), wxSize(260, 20));
	wxStaticText *Descr4 = new wxStaticText(this, -1, "Expansion:", wxPoint(654, 150));
	SearchExpansion = new wxListBox(this, -1, wxPoint(754, 150), wxSize(260, 140), 0, NULL, wxLB_EXTENDED);
	wxStaticText *Descr5 = new wxStaticText(this, -1, "Type:", wxPoint(654, 300));
	SearchType = new wxListBox(this, -1, wxPoint(754, 300), wxSize(260, 140), 0, NULL, wxLB_EXTENDED);
	
	MoveToDeck = new wxButton(this, WD_MOVETODECK, "Card to Deck", wxPoint(1, 654), wxSize(200, 40));
	MoveToSb = new wxButton(this, WD_MOVETOSB, "Card to Sideboard", wxPoint(201, 654), wxSize(200, 40));
	MoveToLib = new wxButton(this, WD_MOVETOLIB, "Card to Library", wxPoint(401, 654), wxSize(200, 40));
	
	//Assign Values	
	
	ListBox->InsertColumn(0, "Name", wxLIST_FORMAT_LEFT, 200);
	ListBox->InsertColumn(1, "Cost", wxLIST_FORMAT_LEFT, 180);

	int i = 0;
	vector<strCard*> Temp = Frame->CardInfo->GetAllCards();
	for(i = 0; i < Temp.size(); i++)
	{
		int Index = ListBox->GetItemCount();
		wxListItem Item;
		Item.SetMask(wxLIST_MASK_TEXT);
		Item.SetBackgroundColour(CardToWxColour(Temp[i]->Cost, Temp[i]->Type));
		Item.SetText(Temp[i]->Name.c_str());
		Item.SetId(Index);
		ListBox->InsertItem(Item);
		ListBox->SetItem(Index, 1, Temp[i]->Cost.c_str());
	}
	
	vector<string*> TempExp = Frame->CardInfo->GetAllExpansions();
	SearchExpansion->Append("(All)");
	SearchExpansion->SetSelection(0);
	for(i = 0; i < TempExp.size(); i++)
		SearchExpansion->Append(TempExp[i]->c_str());
	
	vector<string*> TempType= Frame->CardInfo->GetAllTypes();
	SearchType->Append("(All)");
	SearchType->SetSelection(0);
	for(i = 0; i < TempType.size(); i++)
		SearchType->Append(TempType[i]->c_str());
	
	this->Connect(WD_LISTBOX, wxEVT_COMMAND_LIST_ITEM_SELECTED,
				 (wxObjectEventFunction)(wxEventFunction)(wxListEventFunction)
				 &CWndDatabase::OnSelect);
	this->Connect(WD_SEARCHOK, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDatabase::OnSearch);
	this->Connect(WD_SEARCHRESET, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDatabase::OnSearchReset);
	this->Connect(WD_MOVETODECK, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDatabase::OnMoveTo);
	this->Connect(WD_MOVETOSB, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDatabase::OnMoveTo);
	this->Connect(WD_MOVETOLIB, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDatabase::OnMoveTo);
	this->Connect(-1, wxEVT_LEFT_DOWN, (wxObjectEventFunction)
		(wxEventFunction)(wxMouseEventFunction)&CWndDatabase::OnMouseDown);
}

void CWndDatabase::OnSelect(wxListEvent &Event)
{
	string Item = ListBox->GetItemText(Event.GetIndex()).c_str();
	strCard* Card = prv_Frame->CardInfo->GetCardByName(Item);

	string::size_type pos;
	while(string::npos != (pos = Item.find(" ")))
		Item.replace(pos, 1, "");
	while(string::npos != (pos = Item.find("'")))
		Item.replace(pos, 1, "");
	while(string::npos != (pos = Item.find(",")))
		Item.replace(pos, 1, "");
	while(string::npos != (pos = Item.find("-")))
		Item.replace(pos, 1, "");
	Item = "cardimg/" + Item + ".jpg";

	FILE* FileTest = fopen(Item.c_str(), "r");
	if(NULL != FileTest)
	{
		fclose(FileTest);
		CardWindow->SetBitmap(wxBitmap(wxT(Item.c_str()), wxBITMAP_TYPE_ANY));
	} else {
		CardWindow->SetBitmap(wxBitmap(wxT("img/none.jpg"), wxBITMAP_TYPE_ANY));
	}
	
	CardName->SetValue(Card->Name.c_str());
	CardType->SetValue(Card->Type.c_str());
	CardText->SetValue(Card->Text.c_str());
	CardFlavor->SetValue(Card->Flavor.c_str());
	CardPowTgh->SetValue(Card->PowTgh.c_str());
	CardExpansion->Clear();
	for(int i = 0; i < Card->Expansion.size(); i++)
		CardExpansion->Append(Card->Expansion[i].c_str());
		
	prv_Frame->WndCardInfo->ShowCard(Card->Name);
}

void CWndDatabase::OnSearch(wxCommandEvent& WXUNUSED(event))
{
	ListBox->DeleteAllItems();

	vector<strCard*> Temp = prv_Frame->CardInfo->GetAllCards();
	for(int i = 0; i < Temp.size(); i++)
	{
		if("" != SearchName->GetValue())
			if(string::npos == StringNocaseFind(Temp[i]->Name, SearchName->GetValue().c_str()))
				continue;
		
		if("Must" == SearchManaU->GetValue())
			if(string::npos == Temp[i]->Cost.find("U"))
				continue;
				
		if("Must not" == SearchManaU->GetValue())
			if(string::npos != Temp[i]->Cost.find("U"))
				continue;
				
		if("Must" == SearchManaB->GetValue())
			if(string::npos == Temp[i]->Cost.find("B"))
				continue;
				
		if("Must not" == SearchManaB->GetValue())
			if(string::npos != Temp[i]->Cost.find("B"))
				continue;
				
		if("Must" == SearchManaW->GetValue())
			if(string::npos == Temp[i]->Cost.find("W"))
				continue;
				
		if("Must not" == SearchManaW->GetValue())
			if(string::npos != Temp[i]->Cost.find("W"))
				continue;
				
		if("Must" == SearchManaR->GetValue())
			if(string::npos == Temp[i]->Cost.find("R"))
				continue;
				
		if("Must not" == SearchManaR->GetValue())
			if(string::npos != Temp[i]->Cost.find("R"))
				continue;
				
		if("Must" == SearchManaG->GetValue())
			if(string::npos == Temp[i]->Cost.find("G"))
				continue;
				
		if("Must not" == SearchManaG->GetValue())
			if(string::npos != Temp[i]->Cost.find("G"))
				continue;
				
		if("" != SearchText->GetValue())
			if(string::npos == StringNocaseFind(Temp[i]->Text, SearchText->GetValue().c_str()))
				continue;
				
		if("" != SearchFlavor->GetValue())
			if(string::npos == StringNocaseFind(Temp[i]->Flavor, SearchFlavor->GetValue().c_str()))
				continue;
				
		wxArrayInt ExpanSelections;
		int Expansions = SearchExpansion->GetSelections(ExpanSelections);
		bool ExpFound = false;
		int j = 0;
		for(j = 0; j < Expansions; j++)
		{
			if(0 == ExpanSelections[j])	// (All)
			{
				ExpFound = true;
				break;
			}
			for(int k = 0; k < Temp[i]->Expansion.size(); k++)
			{
				if(Temp[i]->Expansion[k] == SearchExpansion->GetString(ExpanSelections[j]).c_str())
				{
					ExpFound = true;
					break;
				}
			}
		}
		if(!ExpFound)
			continue;
			
		wxArrayInt TypeSelections;
		int Types = SearchType->GetSelections(TypeSelections);
		bool TypeFound = false;
		for(j = 0; j < Types; j++)
		{
			if(0 == TypeSelections[j])	// (All)
			{
				TypeFound = true;
				break;
			}
			if(Temp[i]->Type == SearchType->GetString(TypeSelections[j]).c_str())
			{
				TypeFound = true;
				break;
			}
		}
		if(!TypeFound)
			continue;
		
		int Index = ListBox->GetItemCount();
		wxListItem Item;
		Item.SetMask(wxLIST_MASK_TEXT);
		Item.SetBackgroundColour(CardToWxColour(Temp[i]->Cost, Temp[i]->Type));
		Item.SetText(Temp[i]->Name.c_str());
		Item.SetId(Index);
		ListBox->InsertItem(Item);
		ListBox->SetItem(Index, 1, Temp[i]->Cost.c_str());
	}
}

void CWndDatabase::OnSearchReset(wxCommandEvent& WXUNUSED(event))
{
	ListBox->DeleteAllItems();

	vector<strCard*> Temp = prv_Frame->CardInfo->GetAllCards();
	int i = 0;
	for(i = 0; i < Temp.size(); i++)
	{
		int Index = ListBox->GetItemCount();
		wxListItem Item;
		Item.SetMask(wxLIST_MASK_TEXT);
		Item.SetBackgroundColour(CardToWxColour(Temp[i]->Cost, Temp[i]->Type));
		Item.SetText(Temp[i]->Name.c_str());
		Item.SetId(Index);
		ListBox->InsertItem(Item);
		ListBox->SetItem(Index, 1, Temp[i]->Cost.c_str());
	}
	
	SearchName->SetValue("");
	SearchText->SetValue("");
	SearchFlavor->SetValue("");
	SearchManaU->SetSelection(0);
	SearchManaW->SetSelection(0);
	SearchManaR->SetSelection(0);
	SearchManaG->SetSelection(0);
	SearchManaB->SetSelection(0);
	wxArrayInt ExpanSelections;
	int Expansions = SearchExpansion->GetSelections(ExpanSelections);
	for(i = 0; i < Expansions; i++)
		SearchExpansion->SetSelection(ExpanSelections[i], false);
	SearchExpansion->SetSelection(0);
	wxArrayInt TypeSelections;
	int Types = SearchType->GetSelections(TypeSelections);
	for(i = 0; i < Types; i++)
		SearchType->SetSelection(TypeSelections[i], false);
	SearchType->SetSelection(0);
}

void CWndDatabase::OnMoveTo(wxCommandEvent& event)
{
	string Card = CardName->GetValue().c_str();
	if("" == Card)
		return;

	if(WD_MOVETODECK == event.GetId())
		prv_Frame->WndDeck->AddCard(Card);
	
	if(WD_MOVETOSB == event.GetId())
		prv_Frame->WndDeck->AddCard(Card, true);
	
	if(WD_MOVETOLIB == event.GetId())
		prv_Frame->WndLibrary->AddCard(Card);
}

void CWndDatabase::OnMouseDown(wxMouseEvent& event)
{
	float MouseX = event.GetX();
	float MouseY = event.GetY();
			
	if(event.LeftIsDown() && 
		MouseX > 424 && MouseX < 634 &&
		MouseY > 20 && MouseY < 320)
	{
		string Card = CardName->GetValue().c_str();
			string::size_type pos;
		while(string::npos != (pos = Card.find("'")))
			Card.replace(pos, 1, "");
		while(string::npos != (pos = Card.find(",")))
			Card.replace(pos, 1, "");
		while(string::npos != (pos = Card.find("-")))
			Card.replace(pos, 1, "");
		
		string GetCard = Card;
		while(string::npos != (pos = GetCard.find(" ")))
			GetCard.replace(pos, 1, "_");
		GetCard = "/global/images/magic/general/" + GetCard + ".jpg";
		
		string Dest = Card;
		while(string::npos != (pos = Dest.find(" ")))
			Dest.replace(pos, 1, "");
		Dest = "cardimg/" + Dest + ".jpg";
		
		if(DownloadImage("www.wizards.com", GetCard, Dest))
			cout << GetCard << " to " << Dest << endl;
		else
			cout << "Error during the download of " << GetCard << endl;
	}
}

//-----------------------------------------------------------------------------
//  WndDeck.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The deck page -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

CWndDeck::CWndDeck(MagicFrame* Frame, wxNotebook* parent): 
	wxNotebookPage(parent, -1)
{
	prv_Frame = Frame;
	
	wxStaticBox *Box = new wxStaticBox(this, -1, "Deck", wxPoint(1, 1), wxSize(408, 640));
	ListBox = new wxListCtrl(this, WDD_LISTBOX, wxPoint(4, 20), wxSize(400, 610),  wxLC_REPORT|wxLC_HRULES);
		
	wxStaticBox *Box2 = new wxStaticBox(this, -1, "Import|Export", wxPoint(420, 1), wxSize(598, 640));
	New = new wxButton(this, WDD_BTNNEW, "New", wxPoint(424, 20), wxSize(590, 40));
	ImportApprentice = new wxButton(this, WDD_BTNIMPAPP, "Import Apprentice|Mindless Evolution", wxPoint(424, 60), wxSize(295, 40));
	ExportApprentice = new wxButton(this, WDD_BTNEXPAPP, "Export Apprentice|Mindless Evolution", wxPoint(719, 60), wxSize(295, 40));
	
	//Assign Values	
	
	ListBox->InsertColumn(0, "Name", wxLIST_FORMAT_LEFT, 260);
	ListBox->InsertColumn(1, "Amount", wxLIST_FORMAT_LEFT, 60);
	ListBox->InsertColumn(2, "Sideboard", wxLIST_FORMAT_LEFT, 60);
	
	Menu = new wxMenu("Amount");
	Menu->Append(WDD_MENUI1, "+1");
	Menu->Append(WDD_MENUD1, "-1");
	Menu->Append(WDD_MENUI10, "+10");
	Menu->Append(WDD_MENUD10, "-10");
	
	this->Connect(WDD_BTNNEW, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDeck::OnNew);
	this->Connect(WDD_BTNIMPAPP, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDeck::OnImportApprentice);
	this->Connect(WDD_BTNEXPAPP, wxEVT_COMMAND_BUTTON_CLICKED,
		(wxObjectEventFunction)&CWndDeck::OnExportApprentice);
	this->Connect(WDD_MENUI1, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndDeck::OnAmountI1);
	this->Connect(WDD_MENUD1, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndDeck::OnAmountD1);
	this->Connect(WDD_MENUI10, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndDeck::OnAmountI10);
	this->Connect(WDD_MENUD10, wxEVT_COMMAND_MENU_SELECTED,
		(wxObjectEventFunction)&CWndDeck::OnAmountD10);
	this->Connect(WDD_LISTBOX, wxEVT_COMMAND_LIST_ITEM_SELECTED,
				 (wxObjectEventFunction)(wxEventFunction)(wxListEventFunction)
				 &CWndDeck::OnSelect);
				 
	this->Connect(-1, wxEVT_PAINT, (wxObjectEventFunction)&CWndDeck::OnPaint);
}

void CWndDeck::OnPaint(wxPaintEvent& event)
{
	int Cards = 0;
	int TypeArtifact = 0, TypeLand = 0, TypeCreature = 0;
	int TypeEnchantment = 0, TypeInstant = 0, TypeSorcery = 0;

	long Items = ListBox->GetItemCount();
	for(long i = 0; i < Items; i++)
	{
		wxListItem Item;
		Item.SetMask(wxLIST_MASK_TEXT);
		Item.SetId(i);
		Item.SetColumn(2);
		ListBox->GetItem(Item);
		if("Yes" == Item.GetText())
			next;
		Item.SetColumn(1);
		ListBox->GetItem(Item);
		int amnt = atoi(Item.GetText().c_str());
		Cards += amnt;
		Item.SetColumn(0);
		strCard* Card = prv_Frame->CardInfo->GetCardByName(ListBox->GetItem(Item));
		if(Card->Type == "Enchantment") { TypeEnchantment += amnt; break; } // Enchant * ...
		//if(Card->Type == "") { += amnt; break; }
	}

	wxPaintDC dc(this);
	wxBrush brushArtifact; 		brushArtifact.SetColour(149, 149, 149);
	wxBrush brushLand; 			brushLand.SetColour(162, 103, 11);
	wxBrush brushCreature;		brushCreature.SetColour(154, 194, 163);
	wxBrush brushEnchantment;	brushEnchantment.SetColour(159, 177, 207);
	wxBrush brushInstant;		brushInstant.SetColour(228, 168, 175);
	wxBrush brushSorcery;		brushSorcery.SetColour(207, 221, 51);

	dc.SetBrush(brushArtifact);
	dc.DrawEllipticArc(470, 250, 100, 100, 0, 45);
	dc.SetTextForeground(brushArtifact.GetColour());
	dc.DrawText("Artifact", 580, 250);
	
	dc.SetBrush(brushLand);
	dc.DrawEllipticArc(470, 250, 100, 100, 45, 90);
	dc.SetTextForeground(brushLand.GetColour());
	dc.DrawText("Land", 580, 260);
	
	dc.SetBrush(brushCreature);
	dc.DrawEllipticArc(470, 250, 100, 100, 90, 135);
	dc.SetTextForeground(brushCreature.GetColour());
	dc.DrawText("Creature", 580, 270);
		
	dc.SetBrush(brushEnchantment);
	dc.DrawEllipticArc(470, 250, 100, 100, 135, 180);
	dc.SetTextForeground(brushEnchantment.GetColour());
	dc.DrawText("Enchantment", 580, 280);
		
	dc.SetBrush(brushInstant);
	dc.DrawEllipticArc(470, 250, 100, 100, 180, 225);
	dc.SetTextForeground(brushInstant.GetColour());
	dc.DrawText("Instant", 580, 290);
		
	dc.SetBrush(brushSorcery);
	dc.DrawEllipticArc(470, 250, 100, 100, 225, 270);
	dc.SetTextForeground(brushSorcery.GetColour());
	dc.DrawText("Sorcery", 580, 300);
	


	/*wxBrush brushWhite;		brushWhite.SetColour(255, 255, 255);
	wxBrush brushBlue; 		brushBlue.SetColour(159, 177, 207);
	wxBrush brushRed; 		brushRed.SetColour(228, 168, 175);
	wxBrush brushBlack; 	brushBlack.SetColour(88, 88, 88);
	wxBrush brushGreen; 	brushGreen.SetColour(154, 194, 163);
	wxBrush brushOther; 	brushOther.SetColour(207, 221, 51);
*/
}

void CWndDeck::OnNew(wxCommandEvent& WXUNUSED(event))
{
	ListBox->DeleteAllItems();
}

void CWndDeck::OnImportApprentice(wxCommandEvent& WXUNUSED(event))
{
	wxFileDialog *Dialog = new wxFileDialog(this, "Choose a file", "decks", "", "Apprentice files (*.dec, *.txt)|*.dec;*.txt", wxOPEN);
	if(wxID_OK == Dialog->ShowModal())
	{
		ListBox->DeleteAllItems();
		
		ifstream AppFile(Dialog->GetPath());
		while(!AppFile.eof())
		{
			char Line[1024];
			AppFile.getline(Line, 1024);
			string LineStr = Line;
			
			if("" == LineStr) //ignore blank lines
				continue;
			
			LineStr = LTrim(LineStr);
			
			if("//" == LineStr.substr(0, 2)) //ignore comments
				continue;
			
			bool Sideboard = false;
			if("SB:" == LineStr.substr(0, 3)) //Sideboard card?
			{
				LineStr = LineStr.substr(3, LineStr.size()-3);
				Sideboard = true;
			}
			
			LineStr = LTrim(LineStr);
			
			string::size_type Pos = LineStr.find(' ');
			string Temp = LineStr.substr(0, Pos);
			LineStr = LineStr.substr(Pos+1, LineStr.size()-Pos);
 			long Index = ListBox->GetItemCount();
 			ListBox->InsertItem(Index, LineStr.c_str());
 			ListBox->SetItem(Index, 1, Temp.c_str());
			if(Sideboard)
				ListBox->SetItem(Index, 2, "Yes");
			else
				ListBox->SetItem(Index, 2, "No");
		}
		AppFile.close();
	}
	Dialog->Destroy();
}

void CWndDeck::OnExportApprentice(wxCommandEvent& WXUNUSED(event))
{
	wxFileDialog *Dialog = new wxFileDialog(this, "Choose a file", "decks", "", "Apprentice files (*.dec, *.txt)|*.dec;*.txt", wxSAVE);
	if(wxID_OK == Dialog->ShowModal())
	{
		ofstream AppFile(Dialog->GetPath());
		long Items = ListBox->GetItemCount();
		for(long i = 0; i < Items; i++)
		{
			wxListItem Item;
			Item.SetMask(wxLIST_MASK_TEXT);
			Item.SetId(i);
			Item.SetColumn(2);
			ListBox->GetItem(Item);
			if("Yes" == Item.GetText())
				AppFile << "SB: ";
			Item.SetColumn(1);
			ListBox->GetItem(Item);
			AppFile << Item.GetText().c_str() << " ";
			Item.SetColumn(0);
			ListBox->GetItem(Item);
			AppFile << Item.GetText().c_str() << endl;
		}
		AppFile.close();
	}
	Dialog->Destroy();
}

void CWndDeck::OnSelect(wxListEvent &Event)
{
	this->PopupMenu(Menu, Event.GetPoint());
	prv_Frame->WndCardInfo->ShowCard(Event.GetText().c_str());
}

void CWndDeck::OnAmountI1(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1);
}

void CWndDeck::OnAmountD1(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, true);
}

void CWndDeck::OnAmountI10(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, false, 10);
}

void CWndDeck::OnAmountD10(wxCommandEvent& WXUNUSED(event))
{
	ListCtrlAmountChange(ListBox, ListBox->GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED), 1, true, 10);
}

void CWndDeck::AddCard(string Name, bool Sideboard)
{
	long FindIndex = -1;
	while(-1 != (FindIndex = ListBox->FindItem(FindIndex, Name.c_str())))
	{
		wxListItem Item;
		Item.SetMask(wxLIST_MASK_TEXT);
		Item.SetId(FindIndex);
		Item.SetColumn(2);
		ListBox->GetItem(Item);
		if(("Yes" == Item.GetText() && Sideboard) || ("No" == Item.GetText() && !Sideboard))
		{
			ListCtrlAmountChange(ListBox, FindIndex, 1);
			return;
		}
		FindIndex++;
	}
	
	int Index = ListBox->GetItemCount();
	ListBox->InsertItem(Index, Name.c_str());
	ListBox->SetItem(Index, 1, "1");
	if(Sideboard)
		ListBox->SetItem(Index, 2, "Yes");
	else
		ListBox->SetItem(Index, 2, "No");
}

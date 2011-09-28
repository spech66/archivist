//-----------------------------------------------------------------------------
//  WndDatabase.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The database page -
//-----------------------------------------------------------------------------

#ifndef __WNDDATABASEH__
#define __WNDDATABASEH__

//-----------------------------------------------------------------------------
// Enum
//-----------------------------------------------------------------------------
enum WndDatabaseID
{
	WD_LISTBOX,
	WD_SEARCHOK,
	WD_SEARCHRESET,
	WD_MOVETODECK,
	WD_MOVETOSB,
	WD_MOVETOLIB,	
	WD_STATICBMP,
};

class MagicFrame;

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class CWndDatabase: public wxNotebookPage 
{
	private:
		MagicFrame* prv_Frame;

	protected:
		wxNewBitmap* 	CardWindow;
		wxListCtrl*		ListBox;
		wxTextCtrl*		CardName;
		wxTextCtrl*		CardType;
		wxTextCtrl*		CardText;
		wxTextCtrl*		CardFlavor;
		wxListBox*		CardExpansion;
		wxTextCtrl*		CardPowTgh;
		wxTextCtrl*		SearchName;
		wxComboBox*		SearchManaU;
		wxComboBox*		SearchManaB;
		wxComboBox*		SearchManaW;
		wxComboBox*		SearchManaR;
		wxComboBox*		SearchManaG;
		wxTextCtrl*		SearchText;
		wxTextCtrl*		SearchFlavor;
		wxListBox*		SearchExpansion;
		wxListBox*		SearchType;
		wxButton*		SearchOk;
		wxButton*		SearchReset;
		wxButton*		MoveToDeck;
		wxButton*		MoveToSb;
		wxButton*		MoveToLib;
				
		void OnSelect(wxListEvent &Event);
		void OnSearch(wxCommandEvent& WXUNUSED(event));
		void OnSearchReset(wxCommandEvent& WXUNUSED(event));
		void OnMoveTo(wxCommandEvent& event);
		void OnMouseDown(wxMouseEvent& event);
	
	public:
		CWndDatabase(MagicFrame* Frame, wxNotebook* parent);
};

#endif

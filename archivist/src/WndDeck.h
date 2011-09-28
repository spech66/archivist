//-----------------------------------------------------------------------------
//  WndDeck.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The deck page -
//-----------------------------------------------------------------------------

#ifndef __WNDDECKH__
#define __WNDDECKH__

//-----------------------------------------------------------------------------
// Enum
//-----------------------------------------------------------------------------
enum WndDeckID
{
	WDD_LISTBOX,
	WDD_MENUI1,
	WDD_MENUI10,
	WDD_MENUD1,
	WDD_MENUD10,
	
	WDD_BTNNEW,
	WDD_BTNIMPAPP,
	WDD_BTNEXPAPP,
};

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class CWndDeck: public wxNotebookPage 
{
	private:
		MagicFrame* prv_Frame;
		
	protected:
		wxListCtrl* ListBox;
		wxMenu*		Menu;
		wxButton*	New;
		wxButton*	ImportApprentice;
		wxButton*	ExportApprentice;
				
	public:
				CWndDeck(MagicFrame* Frame, wxNotebook* parent);
		void	OnPaint(wxPaintEvent& event);
		void 	OnNew(wxCommandEvent& WXUNUSED(event));
		void 	OnImportApprentice(wxCommandEvent& WXUNUSED(event));
		void 	OnExportApprentice(wxCommandEvent& WXUNUSED(event));		
		void 	OnSelect(wxListEvent &Event);
		void 	OnAmountI1(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountD1(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountI10(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountD10(wxCommandEvent& WXUNUSED(event));
		void 	AddCard(string Name, bool Sideboard = false);
};

#endif

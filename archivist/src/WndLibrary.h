//-----------------------------------------------------------------------------
//  WndLibrary.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - The library page -
//-----------------------------------------------------------------------------

#ifndef __WNDLIBRARYH__
#define __WNDLIBRARYH__

//-----------------------------------------------------------------------------
// Enum
//-----------------------------------------------------------------------------
enum WndLibraryID
{
	WL_LISTBOX,
	WL_MENUDEL,
	WL_MENUI1,
	WL_MENUI10,
	WL_MENUD1,
	WL_MENUD10,
};

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class CWndLibrary: public wxNotebookPage 
{
	private:
		MagicFrame* prv_Frame;
		
	protected:
		wxListCtrl* ListBox;
		wxMenu*		Menu;
				
	public:
				CWndLibrary(MagicFrame* Frame, wxNotebook* parent);
				~CWndLibrary();
		void 	OnSelect(wxListEvent &Event);
		void 	OnDelete(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountI1(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountD1(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountI10(wxCommandEvent& WXUNUSED(event));
		void 	OnAmountD10(wxCommandEvent& WXUNUSED(event));
		void 	AddCard(string Name);
};

#endif

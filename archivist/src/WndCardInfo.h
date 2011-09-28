//-----------------------------------------------------------------------------
//  WndCardInfo.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Card info page -
//-----------------------------------------------------------------------------

#ifndef __WNDCARDINFOH__
#define __WNDCARDINFOH__

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class CWndCardInfo: public wxNotebookPage 
{
	private:
		MagicFrame* prv_Frame;
		
	protected:
		wxHtmlWindow* HtmlWindow;
				
	public:
				CWndCardInfo(MagicFrame* Frame, wxNotebook* parent);
				void ShowCard(string CardName);
};

#endif

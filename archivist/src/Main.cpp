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

//-----------------------------------------------------------------------------
// wxWidgets App
//-----------------------------------------------------------------------------
IMPLEMENT_APP(MagicApp)

bool MagicApp::OnInit()
{	
	wxImage::AddHandler(new wxJPEGHandler);
	wxImage::AddHandler(new wxGIFHandler);
	MainFrame = new MagicFrame("Archivist - Magic The Gathering", wxSize(1024, 768));
	return true;
}

int MagicApp::OnExit()
{
	return true;
}

MagicFrame::MagicFrame(const wxString& title, const wxSize& size):
						wxFrame((wxFrame*)NULL, -1, title, wxDefaultPosition,
						size, wxDEFAULT_FRAME_STYLE)
{
	CardInfo = new CCardInfo();

	Notebook = new wxNotebook(this, -1);
	WndDatabase = new CWndDatabase(this, Notebook);
	WndDeck = new CWndDeck(this, Notebook);
	WndLibrary = new CWndLibrary(this, Notebook);
	WndCardInfo = new CWndCardInfo(this, Notebook);
	
	wxNotebookPage* About = new wxNotebookPage(Notebook, -1);
	wxStaticBitmap *Logo = new wxStaticBitmap(About, -1, wxBitmap(wxT("img/logo.jpg"), wxBITMAP_TYPE_ANY), wxPoint(0, 0), wxSize(187, 150));
	wxStaticText *Text = new wxStaticText(About, -1, "Copyright by Sebastian Pech", wxPoint(0, 160));
	
	Notebook->AddPage(About, "About", true);
 	Notebook->AddPage(WndDatabase, "Card Database");
	Notebook->AddPage(WndDeck, "Deck");
	Notebook->AddPage(WndLibrary, "Library");
	Notebook->AddPage(WndCardInfo, "Card info");
		
	Show();
}

MagicFrame::~MagicFrame()
{
	if(NULL != CardInfo)
		delete CardInfo;
}

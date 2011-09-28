//-----------------------------------------------------------------------------
// Copyright (C) 2004 Sebastian Pech
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//  Main.h
//  http://www.spech.de
//  - Main File -
//-----------------------------------------------------------------------------

#ifndef __MAINH__
#define __MAINH__

//-----------------------------------------------------------------------------
// wxWidgets Includes
//-----------------------------------------------------------------------------
#include <wx/wx.h>
#include <wx/image.h>
#include <wx/listctrl.h>
#include <wx/notebook.h>
#include <wx/html/htmlwin.h>
#include <wx/brush.h>

#if defined(WIN32)
	#define LF3DPLATFORM_WINDOWS
	#define WIN32_LEAN_AND_MEAN
	#include <windows.h>
	#define snprintf _snprintf
	#include <algorithm>
	#include <winsock2.h>	//Network
#else
	#include <sys/types.h>	//Network
	#include <sys/socket.h>
	#include <netinet/in.h>
	#include <netdb.h>
	#include <arpa/inet.h>
#endif

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <cctype> //transform
using namespace std;

//-----------------------------------------------------------------------------
// Intern Includes
//-----------------------------------------------------------------------------
#include "wxNewBitmap.h"
#include "Helper.h"
#include "CardInfo.h"
#include "WndDatabase.h"
#include "WndDeck.h"
#include "WndLibrary.h"
#include "WndCardInfo.h"

//-----------------------------------------------------------------------------
// Main Frame
//-----------------------------------------------------------------------------
class MagicFrame: wxFrame
{
	private:

	protected:
		wxNotebook*		Notebook;
	
	public:
		CCardInfo*		CardInfo;
		CWndDatabase*	WndDatabase;
		CWndDeck*		WndDeck;
		CWndLibrary*	WndLibrary;
		CWndCardInfo*	WndCardInfo;
				
		MagicFrame(const wxString& title, const wxSize& size);
		~MagicFrame();
};

//-----------------------------------------------------------------------------
// Application
//-----------------------------------------------------------------------------
class MagicApp: public wxApp
{
	MagicFrame *MainFrame;

	virtual bool OnInit();
	virtual int OnExit();
};

#endif

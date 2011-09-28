//-----------------------------------------------------------------------------
//  wxNewBitmap.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Fixes wxStaticBitmap -
//-----------------------------------------------------------------------------

#ifndef __WXNEWBITMAP__
#define __WXNEWBITMAP__

//-----------------------------------------------------------------------------
// Deklaration
//-----------------------------------------------------------------------------
class wxNewBitmap : public wxStaticBitmap
{
	private:
		wxPoint prv_Pos;
		
	public:
		wxNewBitmap(wxWindow* parent, wxWindowID id, const wxBitmap& label,
					const wxPoint& pos, const wxSize& size = wxDefaultSize,
					long style = 0, const wxString& name = "staticBitmap");
		void OnMouseUp(wxMouseEvent& event);
		void OnMouseDown(wxMouseEvent& event);
		void OnMouseMove(wxMouseEvent& event);

		DECLARE_EVENT_TABLE();
};

#endif

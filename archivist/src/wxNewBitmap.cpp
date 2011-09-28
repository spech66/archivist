//-----------------------------------------------------------------------------
//  wxNewBitmap.h
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Fixes wxStaticBitmap -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

//-----------------------------------------------------------------------------
// Event Table
//-----------------------------------------------------------------------------
BEGIN_EVENT_TABLE(wxNewBitmap, wxStaticBitmap)
	EVT_LEFT_DOWN(wxNewBitmap::OnMouseDown)
	EVT_LEFT_UP(wxNewBitmap::OnMouseUp)
	EVT_MOTION(wxNewBitmap::OnMouseMove)
END_EVENT_TABLE()

wxNewBitmap::wxNewBitmap(wxWindow* parent, wxWindowID id, const wxBitmap& label,
						const wxPoint& pos, const wxSize& size, long style,
						const wxString& name)
					: wxStaticBitmap(parent, id, label, pos, size, style, name)
{
	prv_Pos = pos;
}

void wxNewBitmap::OnMouseUp(wxMouseEvent& event)
{
	event.m_x += prv_Pos.x;
	event.m_y += prv_Pos.y;
	m_parent->ProcessEvent(event);
}

void wxNewBitmap::OnMouseDown(wxMouseEvent& event)
{
	event.m_x += prv_Pos.x;
	event.m_y += prv_Pos.y;
	m_parent->ProcessEvent(event);
}

void wxNewBitmap::OnMouseMove(wxMouseEvent& event)
{
	event.m_x += prv_Pos.x;
	event.m_y += prv_Pos.y;
	m_parent->ProcessEvent(event);
}

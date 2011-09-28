//-----------------------------------------------------------------------------
//  WndCardInfo.cpp
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of "Archivist".
// 	For conditions of distribution and use, see copyright notice in Main.h
//  - Card info page -
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Includes
//-----------------------------------------------------------------------------
#include "Main.h"

CWndCardInfo::CWndCardInfo(MagicFrame* Frame, wxNotebook* parent): 
	wxNotebookPage(parent, -1)
{
	prv_Frame = Frame;

	HtmlWindow = new wxHtmlWindow(this, -1, wxPoint(1, 1), wxSize(1016, 640), wxHW_SCROLLBAR_NEVER);
}

void CWndCardInfo::ShowCard(string CardName)
{
	strCard* Card = prv_Frame->CardInfo->GetCardByName(CardName);
	
	string ImageName = CardName;
	string::size_type pos;
	while(string::npos != (pos = ImageName.find(" ")))
		ImageName.replace(pos, 1, "");
	while(string::npos != (pos = ImageName.find("'")))
		ImageName.replace(pos, 1, "");
	while(string::npos != (pos = ImageName.find(",")))
		ImageName.replace(pos, 1, "");
	while(string::npos != (pos = ImageName.find("-")))
		ImageName.replace(pos, 1, "");
	ImageName = "cardimg/" + ImageName + ".jpg";
	FILE* FileTest = fopen(ImageName.c_str(), "r");
	if(NULL != FileTest)
	{
		fclose(FileTest);
	} else {
		ImageName = "img/none.jpg";
	}
	
	string TempCost = Card->Cost;
	string::size_type TCpos;
	while(string::npos != (TCpos = TempCost.find("{")))
		TempCost.replace(TCpos, 1, "<img src=\"img/");
	while(string::npos != (TCpos = TempCost.find("}")))
		TempCost.replace(TCpos, 1, ".gif\"> ");
	transform(TempCost.begin(), TempCost.end(), TempCost.begin(), (int(*)(int))tolower);
		
	string TempText = Card->Text;
	string::size_type TTpos;
	while(string::npos != (TTpos = TempText.find('\n')))
		TempText.replace(TTpos, 1, "<br>");
		
	string TempFlavor = Card->Flavor;
	string::size_type TFpos;
	while(string::npos != (TFpos = TempFlavor.find(";")))
		TempFlavor.replace(TFpos, 1, "<br>");
	
	string String;
	String = "<html><body bgcolor=\"#efefef\"><table width=\"100%\"><tr>";
	String += "<td width=\"20%\" valign=\"top\"><img src=\"" + ImageName + "\" border=\"0\"></td>";
	String += "<td width=\"80%\" valign=\"top\"><table width=\"100%\">";
		String += "<tr><td bgcolor=\"#62d8ea\">";
			String += "<table width=\"100%\"><tr><td>" + Card->Name + "</td>";
			String += "<td align=\"center\">" + TempCost + "</td></tr></table>";
			String += "</td></tr>";
		String += "<tr><td bgcolor=\"#9ce1ec\">" + Card->Type + "</td></tr>";
		String += "<tr><td bgcolor=\"#b9e6ed\">" + TempText + "</td></tr>";
		String += "<tr><td bgcolor=\"#cae9ee\">" + TempFlavor + "</td></tr>";
		String += "<tr><td>" + Card->PowTgh + "</td></tr>";
	String += "</table></td>";
	String += "</tr></table></body></html>";
	HtmlWindow->SetPage(String.c_str());
}

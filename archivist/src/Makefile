PROGRAM = ../archivist
OBJECTS = Main.o Helper.o CardInfo.o WndDatabase.o WndDeck.o WndLibrary.o WndCardInfo.o wxNewBitmap.o
CXX = g++
LDDFLAGS = -Wl -ldl
CXXFLAGS = -g -export-dynamic `wx-config --cxxflags`
INCLUDES = -I/usr/X11R6/include
LIBS = `wx-config --libs`

all: $(PROGRAM)
$(PROGRAM) : $(OBJECTS)
	$(CXX) $(LDDFLAGS) -o $@ $(LIBS) $(OBJECTS)

%.o: %.cpp %.h
	$(CXX) -c $< -o $@ $(INCLUDES) $(CXXFLAGS)

.PHONY : clean
clean :
	rm -f $(PROGRAM) $(OBJECTS)

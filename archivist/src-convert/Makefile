PROGRAM = ../data/oracleconv
OBJECTS = Main.o
CXX = g++
LDDFLAGS = -Wl -ldl
CXXFLAGS = -g -export-dynamic
INCLUDES = -I/usr/X11R6/include
LIBS =

all: $(PROGRAM)
$(PROGRAM) : $(OBJECTS)
	$(CXX) $(LDDFLAGS) -o $@ $(LIBS) $(OBJECTS)

%.o: %.cpp %.h
	$(CXX) -c $< -o $@ $(INCLUDES) $(CXXFLAGS)

.PHONY : clean
clean :
	rm -f $(PROGRAM) $(OBJECTS)

\ Advent of Code 2020 day 1 part 2

decimal

[undefined] cell [if] 1 cells constant cell [then]

\ Define stdin as a local file if not available in your forth
[undefined] stdin [if] s" 1.dat" r/o open-file throw constant stdin [then]

\ Read line from file or throw exception
: read-line! ( c-addr u fid -- n )  read-line throw drop ;

\ Read a line to pad, return size
: line>pad ( -- n )  pad 80 stdin read-line! ;

\ Add all numbers in input to an array
: read-all ( -- )  begin line>pad ?dup while pad swap evaluate , repeat ;

\ Add all numbers in input to an array and stash size in first cell
: read-all-arr ( "name" -- )
	create here >r 0 , read-all here r@ - cell / 1- r> !
does>
	cell+ ;

\ Get stashed number of elements
: arr-count ( addr -- addr u ) dup -1 cells + @ ;

\ Get do-style bounds for a stashed-size array
: arr-bounds ( addr -- end-addr addr ) arr-count cells over + swap ;

\ arr-do/arr-loop for iterating over stashed-size array's elements
: arr-do ( ex: addr -- ) postpone arr-bounds postpone ?do ; immediate
: arr-loop ( ex: -- )  postpone cell postpone +loop ; immediate

read-all-arr numbers

\ Is a number in array?
: n-in? ( n addr -- false | n true )
	arr-do
		dup i @ = if true unloop exit then
	arr-loop
	drop false ;

\ Try solving given two numbers
: find-for-2020 ( n1 n2 -- false | n1 n2 n3 true )
	2dup + 2020 swap - numbers n-in? if true else 2drop false then ;

\ Search for the three numbers adding to 2020 in numbers array
: find-3-2020 ( -- n1 n2 n3 )
	numbers arr-do numbers arr-do
		i @ j @ find-for-2020 if unloop unloop exit then
	arr-loop arr-loop ;

\ Find three adding to 2020 and print product
find-3-2020 * * u. cr

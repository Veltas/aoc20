\ Advent of Code 2020 day 1 part 1

\ Define stdin as a local file if not available in your forth
[undefined] stdin [if] s" 1.dat" r/o open-file throw constant stdin [then]

decimal

\ Read line from file or abort
: aread-line ( c-addr u fid -- n )  read-line if abort" read fail" then drop ;

\ Read a line to pad, return size
: line>pad ( -- n )  pad 80 stdin aread-line ;

\ Add all numbers in input to current array
: read-all ( -- )  begin line>pad ?dup while pad swap evaluate , repeat ;

create numbers here read-all

\ Size of array
here swap - 1 cells / constant #numbers

\ Is a number in array?
: n-in? ( n addr u -- true|false )
	cells over + swap  ?do
		dup i @ = if drop true unloop exit then
	1 cells +loop
	drop false ;

\ Search to ensure no number adding to 2020 with n1
: none-for-2020 ( n1 -- n1 n2 false | true )
	dup 2020 swap - ( n1 n2 )
	dup numbers #numbers n-in? if false else 2drop true then ;

\ Search for numbers adding to 2020 in numbers array
: find-2020 ( -- n1 n2 )
	numbers begin dup @ none-for-2020 while cell+ repeat rot drop ;

\ Find pair adding to 2020 and print product
find-2020 * u. cr

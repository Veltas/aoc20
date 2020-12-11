\ Advent of Code 2020 day 1 part 2 for big dataset

decimal

99920044 constant target

[undefined] -rot [if] : -rot ( a b c -- c a b )  rot rot ; [then]

[undefined] cell [if] 1 cells constant cell [then]

\ Define stdin as a local file if not available in your forth
[undefined] stdin [if] s" 1.dat" r/o open-file throw constant stdin [then]

\ Read line from file or throw exception
: read-line! ( c-addr u fid -- n )  read-line throw drop ;

\ Read a line to pad, return size
: line>pad ( -- n )  pad 80 stdin read-line! ;

\ Add all numbers in input to an array
: read-all ( -- )  begin line>pad ?dup while pad swap evaluate , repeat ;

\ Split array in half
: split-arr ( a u -- a1 u/2 a2 u-u/2 )
	dup >r  1 rshift  2dup  swap 2 pick cells +  swap r> swap - ;

: some|drop ( a u -- a u true | false )  dup if true else 2drop false then ;

\ Check neither array is empty, otherwise drop empty array
: both-some? ( a1 u1 a2 u2 -- a u false | a1 u1 a2 u2 true )
	2swap dup if 2swap then some|drop ;

\ Pop one element from an array
: pop-arr ( a1 u1 -- a1+cell u1-1 x )  1- swap dup cell+ -rot @ ;

\ Compare first elements of array
: x1>x2 ( a1 u1 a2 u2 -- flags )  drop @ nip swap @ < ;

\ Pop smaller element from either of two arrays to data-space
: insert-smaller ( a1 u1 a2 u2 -- a3 u3 a4 u4 )
	 2over 2over x1>x2 if pop-arr , else 2swap pop-arr , 2swap then ;

\ Pop array to data-space until empty
: insert-rest ( a u -- )  begin dup while pop-arr , repeat 2drop ;

\ Remove data-space starting at buf
: delete ( buf -- )  here - allot ;

\ Merge sorted split arrays into data area
: merge-insert ( addr1 u1 addr2 u2 -- )
	begin both-some? while insert-smaller repeat insert-rest ;

\ Copy buf to addr and delete
: move-back ( addr buf -- )  tuck aligned swap over here swap - move delete ;

\ Merge sorted split arrays
: merge ( addr1 u1 addr2 u2 -- )
	3 pick  here  2>r  align merge-insert  2r> move-back ;

\ Sort array of cells with "merge sort" algorithm
: sort ( addr u -- )
	dup 1 > if
		split-arr 2swap 2dup recurse 2swap 2dup recurse merge
	else
		2drop
	then ;

\ Add all numbers in input to an array and stash size in first cell
: read-all-arr ( "name" -- )
	create here >r 0 , read-all here r@ - cell / 1- r> !
does>
	cell+ ;

\ Get stashed number of elements
: arr-count ( addr -- addr u ) dup -1 cells + @ ;

: arr-last ( addr -- addr-last ) arr-count 1- cells + ;

: arr-loop ( ex: -- )  postpone cell postpone +loop ; immediate

\ Read then sort all numbers
read-all-arr numbers
numbers arr-count sort

\ Search for the three numbers adding to target in numbers array
: find-3-target ( -- n1 n2 n3 )
	-1  numbers  do
		numbers arr-last
		dup  i cell+  ?do
			i @ j @ + swap
			begin
				dup i > if
					2dup @ + target >
				else
					false
				then
			while
				cell -
			repeat
			dup i = if drop leave then
			2dup @ + target = if
				nip @ i @ j @ unloop unloop exit
			then
			nip
		arr-loop
		drop
	arr-loop ;

\ Find three adding to target, print 3 numbers then product
find-3-target  .s cr  * * u. cr

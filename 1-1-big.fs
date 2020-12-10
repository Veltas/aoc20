\ Advent of Code 2020 day 1 part 1 for big dataset

decimal

99920044 constant target

[undefined] -rot [if] : -rot ( a b c -- c a b )  rot rot ; [then]

\ Define stdin as a local file if not available in your forth
[undefined] stdin [if] s" 1.dat" r/o open-file throw constant stdin [then]

\ Read line from file or abort
: aread-line ( c-addr u fid -- n )  read-line if abort" read fail" then drop ;

\ Read a line to pad, return size
: line>pad ( -- n )  pad 80 stdin aread-line ;

\ Add all numbers in input to current array
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

: 3dup ( a b c -- a b c a b c )  >r 2dup r@ -rot r> ;

: 3drop ( a b c -- )  2drop drop ;

: mid@ ( addr u -- x )  1 rshift cells + @ ;

: below-mid ( a u -- a u/2 )  1 rshift ;

: above-mid ( a u -- a2 u2 )
	dup 1 rshift >r  swap r@ 1+ cells +  swap r> - 1- ;

defer n-in?

: n-in?-not-mid ( n addr u -- true|false )
	3dup mid@ < if below-mid n-in? else above-mid n-in? then ;

: n-in?-not-empty ( n addr u -- true|false )
	3dup mid@ = if 3drop true else n-in?-not-mid then ;

\ Is a number in sorted array?
:noname ( n addr u -- true|false )
	?dup if n-in?-not-empty else 2drop false then ; is n-in?

create numbers here read-all

\ Size of array
here swap - 1 cells / constant #numbers

numbers #numbers sort

\ Search to ensure no number adding to target with n1
: none-for-target ( n1 -- n1 n2 false | true )
	dup target swap - ( n1 n2 )
	dup numbers #numbers n-in? if false else 2drop true then ;

\ Search for numbers adding to target in numbers array
: find-target ( -- n1 n2 )
	numbers begin dup @ none-for-target while cell+ repeat rot drop ;

\ Find pair adding to target and print product
find-target .s cr * u. cr

\ Advent of Code 2020 day 3 part 1

decimal

[undefined] stdin [if] s" 3.dat" r/o open-file throw constant stdin [then]

0 value #pad

: read-line! ( c-addr u fid -- n )  read-line throw drop ;

: line>pad ( -- )  pad 80 stdin read-line! to #pad ;

: tree?+ ( trees col -- trees|trees+1 )  chars pad + c@ '#' = if 1+ then ;

: col+ ( col -- next-col )  3 + #pad mod ;

: #trees ( -- n )
	0 0 begin line>pad #pad while over tree?+ swap col+ swap repeat nip ;

#trees . cr

\ Advent of Code 2020 day 2 part 1

decimal

\ Define stdin as a local file if not available in your forth
[undefined] stdin [if] s" 2.dat" r/o open-file throw constant stdin [then]

\ Keep current size of pad string
0 value #pad

\ Read line from file or abort
: aread-line ( c-addr u fid -- n )  read-line if abort" read fail" then drop ;

\ Current index being parsed
0 value >pad

\ Read a line to pad, set #pad and >pad
: line>pad ( -- )  pad 80 stdin aread-line to #pad  0 to >pad ;

\ Safely get next character in pad
: c-peek ( -- c | -1 )  >pad #pad = if -1 else pad >pad chars + c@ then ;

\ Move to next character in pad
: c-next ( -- )  >pad 1+ to >pad ;

\ Is character a digit?
: c-digit? ( c -- flags )  '0' '9' 1+ within ;

\ Move through a number in pad
: pass-num ( -- )  begin c-peek c-digit? while c-next repeat ;

\ Get number string in pad
: scan-num ( -- str u )  pad >pad chars +  >pad  pass-num  >pad swap - ;

\ Read a number from pad
: read-num ( -- n )  scan-num evaluate ;

\ Read a password rule from pad, finish right before the password
: read-rule ( -- n1 n2+1 c )
	read-num
	c-next read-num 1+
	c-next c-peek
	c-next c-next ;

\ Return unparsed part of pad
: remaining-pad ( -- str u )  pad >pad chars +  #pad >pad - ;

\ How many times does given character appear in the string?
: ccount ( c str u -- n )
	0 rot rot
	chars over + swap ?do
		over i c@ = if 1+ then
	1 chars +loop
	nip ;

\ Is a password entry valid?
: valid-entry? ( -- flags )  read-rule remaining-pad ccount rot rot within ;

\ How many password entries are valid in all input?
: #valid ( -- n )  0 begin line>pad #pad while valid-entry? if 1+ then repeat ;

\ Print number of valid entries
#valid u. cr

$(document).ready(function($){

    var crotation;
    var rotateto = 0;
    var itemCount = $('item').length; // count of items in corousel
    var tcItemInitialRotation = 360/itemCount;
    var tcZDistance = 200;
    
    $('item').each( function(index) {
                   
        $(this).css({
            'transform' : 'rotateY('+( tcItemInitialRotation * index )+'deg) translateZ('+tcZDistance+'px)'
        }).attr('tc-rotation', ( tcItemInitialRotation * index ) );

    });
    
    function tcRotate(tcdeg){
        $('#container').css({
            'transform'         : 'rotateY('+ tcdeg +'deg)',
            '-ms-transform'     : 'rotateY('+ tcdeg +'deg)',
            '-webkit-transform' : 'rotateY('+ tcdeg +'deg)'
        });
    }
    
    $('item').on('click', function(){
        crotation = $('#container').attr('tcc-rotation');
        var rotation = $(this).attr('tc-rotation');       
        rotateto = crotation - rotation;
        tcRotate(rotateto);
        crotation = rotateto;
    });
     function xunhuan()
    {
    	 rotateto -= tcItemInitialRotation;
    	tcRotate(rotateto);
    }
    
	setInterval(xunhuan,2000);


});
   

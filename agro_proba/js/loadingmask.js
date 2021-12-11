(function( $ )
{
	var methods = {
		show : function( options ) {
			return this.each(function(){
				var $this = $(this);
				var duration = 300;
				var message = '<div class="lm-message">Loading...</div>';

				var content = '<div class="lm-content"><div class="lm-circle">' +
								'<div class="lm-circle1 lm-circle-child"></div>'+
								'<div class="lm-circle2 lm-circle-child"></div>'+
								'<div class="lm-circle3 lm-circle-child"></div>'+
								'<div class="lm-circle4 lm-circle-child"></div>'+
								'<div class="lm-circle5 lm-circle-child"></div>'+
								'<div class="lm-circle6 lm-circle-child"></div>'+
								'<div class="lm-circle7 lm-circle-child"></div>'+
								'<div class="lm-circle8 lm-circle-child"></div>'+
								'<div class="lm-circle9 lm-circle-child"></div>'+
								'<div class="lm-circle10 lm-circle-child"></div>'+
								'<div class="lm-circle11 lm-circle-child"></div>'+
								'<div class="lm-circle12 lm-circle-child"></div>'+
							   '</div></div>';

				var container = $('<div />', { id: 'lm-container' });

				var loadingmask = $('<div />', { id: 'loadingmask' });
				
				if(options){
				    if (options.hasOwnProperty("duration")) {
				        duration = options.duration;
                    }
					if(options.hasOwnProperty("message")) {
					    message = '<div class="lm-message">' + options.message + '</div>';
                    }
				}

				container.html(content + message);
				container.appendTo(loadingmask);
				
				$this.find('#loadingmask').remove();
				loadingmask.appendTo($this).fadeIn(duration);
			});
		},
		hide : function( ) {
			return this.each(function(){
				var $this = $(this);
				$this.find('#loadingmask').fadeOut(300, function(){
                    this.remove();
                });
			});
		}
	};

	$.fn.LoadingMask = function( method ) {
		if ( methods[method] ) {
			return methods[method].apply( this, Array.prototype.slice.call( arguments, 1 ));
		} else if ( typeof method === 'object' || ! method ) {
			return methods.show.apply( this, arguments );
		} else {
			$.error( 'Метод с именем ' +  method + ' не существует для jQuery.LoadingMask' );
		}
	};
})( jQuery );
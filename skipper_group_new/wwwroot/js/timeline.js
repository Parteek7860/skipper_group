(function ($) {
    $(document).ready(function () {
        $('.timeline .more a').click(function () {
            $(this).parents('.item').addClass('more-view');
        });

        $('.timeline .more.back a').click(function () {
            $(this).parents('.item').removeClass('more-view');
        });

        var sync1 = $('.timeline-carousel');
        var sync2 = $('.timeline-num-car');

        var thumbnailItemClass = '.owl-item'; // wrapper for each slide
        var dotSelector = '.owl-dot';         // visible dot inside each slide

        var slides = sync1.owlCarousel({
            loop: false,
            dots: false,
            smartSpeed: 1500,
            touchDrag: false,
            mouseDrag: false,
            animateOut: 'fadeOut',
            animateIn: 'fadeIn',
            items: 1,
            navText: ['<span class="arw-left red"></span>', '<span class="arw-right red"></span>'],
            responsive: {
                0: {
                    items: 1,
                    touchDrag: false,
                    mouseDrag: false,
                },
                600: {
                    touchDrag: false,
                    mouseDrag: false,
                }
            }
        }).on('changed.owl.carousel', syncPosition);

        var allowTransitionLeft = false;
        var allowTransitionRight = true;
var enableScrollNavigation = false; // Set to false to pause

if ($(window).width() >= 768 && enableScrollNavigation) {
    sync1.on('DOMMouseScroll mousewheel', function (event) {
        if (event.originalEvent.detail > 0 || event.originalEvent.wheelDelta < 0) {
            if (allowTransitionRight) {
                allowTransitionRight = false;
                sync1.trigger('next.owl');
            }
        } else {
            if (allowTransitionLeft) {
                allowTransitionLeft = false;
                sync1.trigger('prev.owl');
            }
        }
        return false;
    }).on('translated.owl.carousel', function (e) {
        allowTransitionRight = (e.item.count > e.item.index);
        allowTransitionLeft = (e.item.index > 0);
    });
}

        sync1.on('translate.owl.carousel', function (e) {
            var index = e.item.index,
                target = $(e.target).find(".owl-item"),
                elep = target.eq(index).find("p.desc"),
                eleh3 = target.eq(index).find("h3.op1"),
                imgp1 = target.eq(index).find("img.p1"),
                imgp2 = target.eq(index).find("img.p2"),
                imgp3 = target.eq(index).find("img.p3");

            $('.timeline h2.hd3').removeClass('animated fadeInUp delay1');
            $('.timeline h2.hd3').eq(index).addClass('animated fadeInUp delay1');
            $('.timeline h4.hd1').removeClass('animated fadeInUp delay2');
            $('.timeline h4.hd1').eq(index).addClass('animated fadeInUp delay2');
            $(target).find("p.desc").removeClass('animated fadeInUp delay3');
            $(elep).addClass('animated fadeInUp delay3');
            $(target).find("h3.op1").removeClass('animated fadeInUp delay3');
            $(eleh3).addClass('animated fadeInUp delay3');
            $(target).find("img.p1").removeClass('animated zoomIn delay3');
            $(imgp1).addClass('animated zoomIn delay3');
            $('img.p2').removeClass('animated zoomIn delay4');
            $(imgp2).addClass('animated zoomIn delay4');
            $('img.p3').removeClass('animated zoomIn delay5');
            $(imgp3).addClass('animated zoomIn delay5');

            if ($(window).width() < 768) {
                var mobImg = target.eq(index).find("figure.visible-sm"),
                    eleMore = target.eq(index).find(".more");

                $('.timeline .more').removeClass('animated fadeInUp delay2');
                $(eleMore).addClass('animated fadeInUp delay2');
                $('figure.visible-xs').removeClass('animated fadeInUp delay4');
                $(mobImg).addClass('animated fadeInUp delay4');
            }
        });

        function syncPosition(el) {
            var $owlSlider = $(this).data('owl.carousel');
            var loop = $owlSlider.options.loop;
            var current = loop
                ? Math.round(el.item.index - (el.item.count / 2) - .5)
                : el.item.index;

            if (loop) {
                var count = el.item.count - 1;
                if (current < 0) current = count;
                if (current > count) current = 0;
            }

            var owl_thumbnail = sync2.data('owl.carousel');
            var itemClass = "." + owl_thumbnail.options.itemClass; // ".owl-item"

            var $thumbItems = sync2.find(itemClass);
            $thumbItems.removeClass("synced");
            $thumbItems.eq(current).addClass("synced");

            sync2.find(dotSelector).removeClass('active');
            $thumbItems.eq(current).find(dotSelector).addClass('active');

            if (!$thumbItems.eq(current).hasClass('active')) {
                var duration = 300;
                sync2.trigger('to.owl.carousel', [current, duration, true]);
            }
        }

        var thumbs = sync2.owlCarousel({
            items: 55,
            loop: false,
            margin: 10,
            autoplay: false,
            nav: true,
            navText: ['<span class="arw-left red"></span>', '<span class="arw-right red"></span>'],
            dots: false,
            responsive: {
                0: {
                    items: 25,
                    touchDrag: false,
                    mouseDrag: false,
                },
                600: {
                    items: 25,
                    touchDrag: false,
                    mouseDrag: false,
                }
            },
            onInitialized: function (e) {
                var $owl = $(e.target);
                var current = this._current || 0;

                $owl.find(dotSelector).removeClass('active');
                $owl.find(thumbnailItemClass).eq(current).find(dotSelector).addClass('active');

                $owl.find(thumbnailItemClass).removeClass('synced')
                    .eq(current).addClass('synced');
            },
        }).on('click', thumbnailItemClass, function (e) {
            e.preventDefault();
            var duration = 300;
            var itemIndex = $(e.target).closest(thumbnailItemClass).index();
            sync1.trigger('to.owl.carousel', [itemIndex, duration, true]);
        }).on("changed.owl.carousel", function (el) {
            var number = el.item.index;
            var $owl = $(el.target);

            $owl.find(dotSelector).removeClass('active');
            $owl.find(thumbnailItemClass).eq(number).find(dotSelector).addClass('active');
        });

        $('.container-time').each(function () {
            var _this = $(this);
            var class_text = _this.find('svg').attr('id');
            var id = "1";
            if (typeof class_text != "undefined") {
                if (class_text == 'canvas') {
                    id = "1";
                } else {
                    id = class_text.substring(6);
                }
                _this.addClass('slide' + id);
            }
        });
    });
}(jQuery));

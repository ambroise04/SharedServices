/**
 * Jquery loader displays.
 * Note:~ It is strongly recommended to only call this on
 *  elements which are `position: relative`. If unsure, insert
 *  a `.css({position: 'relative'})` before calling this to enforce
 *  relative positioning. Non-relative parents may have their loaders 
 *  appear outside of their boundaries due to CSS `absolute` 
 *  functionality.
 * 
 * Current features:
 * `create` loader elements in selected elements.
 * `show`/`hide`/`toggle` a loader within an element.
 * 
 * Upcoming features:
 * `error` content to overlay in the event of a failure.
 *  Pass either a callback to return HTML/jQuery object/string, 
 *  or just pass a string to place inside.
 */

var jqloaderDefaults = {
    loaderClass: 'jloader',
    loaderIconClass: 'jloader-icon',
    loaderErrorClass: 'jloader-err',
    loaderContentClass: 'jloader-cont',
    loaderFreezeClass: 'jloader-frz',
};

(function($) {
    var fn = {
        addContent: function($parent, content) {
            /**
             * Clear the `$parent` and place `content` inside.
             */
            $parent.empty();
            if (typeof content === "string") $parent.append("<p>" + content + "</p>");
            else if (typeof content === "function") $parent.append(content());
            else if (content instanceof $) $parent.append(content);
        },
    };

    $.fn.jloader = function(action, content) {
        /**
         * Functionality for loader elements.
         * Only use the first loader found, we don't want all loaders
         * switched every time.
         */
        return this.each(function() {
            // var
            var lcls = jqloaderDefaults.loaderClass;
            var $loader = $(this).find("." + lcls + ":first"),
            ccls = jqloaderDefaults.loaderContentClass,
            ecls = jqloaderDefaults.loaderErrorClass,
            icls = jqloaderDefaults.loaderIconClass,
            frzcls = jqloaderDefaults.loaderFreezeClass;

            if (!action || action === "create") {
                // Default action is to create a loader.
                if ($loader.length < 1) {
                    $loader = $("<div></div>");
                    $loader.addClass(lcls)
                        .hide()
                        .append("<div class=\"" + icls + "\"></div>")
                        .append("<div class=\"" + ccls + "\"></div>")
                        .append("<div class=\"" + ecls + "\" style=\"display: none;\"></div>");
                    
                        fn.addContent($loader.find("." + ccls), content);

                    $(this).prepend($loader);
                }  // else nothing

            } else if (action === "show") {
                // Show loader
                if ($loader.length > 0) {
                    $loader.find("." + icls).removeClass(frzcls).show();
                    $loader.find("." + ecls).hide();
                    $loader.find("." + ccls).show();

                    fn.addContent($loader.find("." + ccls), content);

                    $loader.show();
                }

            } else if (action === "hide") {
                // Hide loader
                if ($loader.length > 0) $loader.hide();

            } else if (action === "toggle") {
                // Toggle loader
                if ($loader.length > 0) {
                    if ($loader.is(":visible")) {
                        $loader.hide();
                    } else {
                        $loader.find("." + icls).removeClass(frzcls).show();
                        $loader.find("." + ecls).hide();
                        $loader.find("." + ccls).show();
                        $loader.show();
                    }
                }

            } else if (action === "error") {
                // Show an error in the overlay
                if ($loader.length > 0) {
                    $loader.find("." + icls).addClass(frzcls).show();
                    $loader.find("." + ecls).show();
                    $loader.find("." + ccls).hide();

                    if (!content) content = "Something went wrong...";
                    fn.addContent($loader.find("." + ecls), content);

                    $loader.show();
                }

            }
        });
    };
}(jQuery));

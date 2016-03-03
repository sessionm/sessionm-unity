#
# Be sure to run `pod lib lint NAME.podspec' to ensure this is a
# valid spec and remove all comments before submitting the spec.
#
# To learn more about a Podspec see http://guides.cocoapods.org/syntax/podspec.html
#
Pod::Spec.new do |s|
  s.name             = "SessionM"
  s.version          = "1.15.5"
  s.summary          = "SessionM SDK"
  s.summary          = "The SessionM SDK provides the world\'s leading loyalty platform."
  s.homepage         = "http://www.sessionm.com"
  s.license          = { :type => 'Commercial', :text => 'Developer\'s use of the SDK is governed by the license in the applicable SessionM Terms of Service.'}
  s.source  = { :path => "SessionM_MMC_iOS_v1.15.5_Release"}
  s.authors = 'The SessionM Team'
  s.platform = :ios
  s.ios.deployment_target = '6.0'
  s.requires_arc = true
  s.source_files = "SessionM_MMC_iOS_v1.15.5_Release/SessionM-SDK/API/*.h"
  s.preserve_paths = "SessionM_MMC_iOS_v1.15.5_Release/SessionM-SDK/libSessionM.1.15.5.a"
  s.vendored_libraries = "SessionM_MMC_iOS_v1.15.5_Release/SessionM-SDK/libSessionM.1.15.5.a"
  s.frameworks =  "ImageIO","AdSupport","AVFoundation","CoreMedia","EventKit","EventKitUI","Security","StoreKit","CoreGraphics","SystemConfiguration","CoreData","Foundation","UIKit"
  s.xcconfig = {
    "LIBRARY_SEARCH_PATHS" => "\"$(PODS_ROOT)/SessionM/**\""
  }
end

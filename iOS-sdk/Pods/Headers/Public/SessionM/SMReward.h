//
//  SMReward.h
//  SessionM
//
//  Copyright (c) 2015 SessionM. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 @class SMReward
 @abstract Defines the data associated with a reward that can redeemed by the user.
 */
@interface SMReward : NSObject

/*!
 @property rewardID
 @abstract Unique reward identifier.
 */
@property(nonatomic, strong, readonly) NSString *rewardID;
/*!
 @property type
 @abstract Reward type.
 */
@property(nonatomic, strong, readonly) NSString *type;
/*!
 @property tier
 @abstract The tier that the user must achieve before the reward will become available for redemption.
 @discussion Some rewards require the user to achieve a certain tier in order to redeem them. A value of <code>nil</code> means that the user can redeem the reward at any time.
 */
@property(nonatomic, strong, readonly) NSString *tier;
/*!
 @property name
 @abstract Reward name.
 */
@property(nonatomic, strong, readonly) NSString *name;
/*!
 @property imageURL
 @abstract URL that points to the promotional image associated with the reward.
 */
@property(nonatomic, strong, readonly) NSString *imageURL;
/*!
 @property url
 @abstract URL that points to the Rewards Store portal page for the reward.
 @discussion Use this URL with the @link //apple_ref/occ/instm/SessionM/presentActivity:withURL: @/link method to deep link to the reward in the portal.
 */
@property(nonatomic, strong, readonly) NSString *url;
/*!
 @property pointValue
 @abstract Amount of points needed to redeem the reward.
 */
@property(nonatomic, strong, readonly) NSNumber *pointValue;
/*!
 @property validUntil
 @abstract Date after which the reward can no longer be redeemed.
 */
@property(nonatomic, strong, readonly) NSString *validUntil;

@end
